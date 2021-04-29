using ECommerce.API.Core;
using ECommerce.API.Core.Configuration;
using ECommerce.Identidade.API.Data;
using ECommerce.Identidade.API.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Identidade.API.Configuration
{
    public static class IdentityConfig
    {
        public static IServiceCollection RegisterIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("Default"));
            });

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMessagePtBr>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.RegisterJWT(configuration);

            return services;
        }

        public static IApplicationBuilder RegisterIdentity(this IApplicationBuilder app)
        {
            return app.RegisterAuthenticationAndConfiguration();
        }
    }
}
