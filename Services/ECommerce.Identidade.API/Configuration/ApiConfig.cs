using ECommerce.Identidade.API.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ECommerce.Identidade.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IGerarRespostaToken, GerarRespostaToken>();

            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder RegisterApp(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.RegisterIdentity();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            return app;
        }
    }
}
