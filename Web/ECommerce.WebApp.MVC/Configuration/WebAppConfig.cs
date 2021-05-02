using ECommerce.WebApp.MVC.Extensions;
using ECommerce.WebApp.MVC.Services;
using ECommerce.WebApp.MVC.Services.Handlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Globalization;

namespace ECommerce.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();

            //services.AddHttpClient<ICatalogoService, CatalogoService>()
            //    .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient("Refit", options =>
                    {
                        options.BaseAddress = new Uri(configuration.GetSection("CatalogoUrl").Value);
                    })
                .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
                .AddTypedClient(Refit.RestService.For<ICatalogoServiceRefit>)
                .AddPolicyHandler(PollyExtension.EspereETente())
                .AddTransientHttpErrorPolicy(x => x.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            //.AddTransientHttpErrorPolicy(opt => opt.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(5), (outcome, timespan, retryCount, context) =>
            //{
            //    Console.ForegroundColor = ConsoleColor.Red;
            //    Console.WriteLine($"Tentando pela {retryCount} vez");
            //    Console.ForegroundColor = ConsoleColor.White;
            //})
            //);

            services.AddScoped<IUser, AspNetUser>();
            services.AddControllersWithViews();
            services.Configure<AppSettings>(configuration);

            return services;
        }

        public static IApplicationBuilder RegisterApplication(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            app.UseExceptionHandler("/erro/500");
            app.UseStatusCodePagesWithRedirects("/erro/{0}");
            app.UseHsts();
            //}

            app.UseStaticFiles();
            app.UseRouting();


            app.RegisterIdentity();

            //Configure de cultura - dá pra colocar outras culturas e trocar depois  (ex: passar a cultura pela rota)
            //Uma vez que eu forço isso aqui ele ignora os dados da máquina ou servidor aonde está hospeadado
            var supportedCultures = new[] { new CultureInfo("pt-BR") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("pt-BR"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });


            app.UseMiddleware<MiddlewareException>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Catalogo}/{action=Index}/{id?}");
            });

            return app;
        }

    }
}
