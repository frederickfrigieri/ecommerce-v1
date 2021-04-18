using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace ECommerce.Identidade.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Contact = new OpenApiContact
                    {
                        Email = "frederick.frigieri@gmail.com",
                        Name = "Frederick Frigieri (Fred)",
                        Url = new Uri("https://github.com/frederickfrigieri")
                    },
                    Description = "ECommerce",
                    Version = "1.0.0"
                });
            });

            return services;
        }

        public static IApplicationBuilder RegisterSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });

            return app;
        }
    }
}
