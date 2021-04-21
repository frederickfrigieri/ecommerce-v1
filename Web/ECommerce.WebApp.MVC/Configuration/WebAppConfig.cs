﻿using ECommerce.WebApp.MVC.Extensions;
using ECommerce.WebApp.MVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.WebApp.MVC.Configuration
{
    public static class WebAppConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient<IAutenticacaoService, AutenticacaoService>();
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
            app.UseMiddleware<MiddlewareException>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            return app;
        }

    }
}
