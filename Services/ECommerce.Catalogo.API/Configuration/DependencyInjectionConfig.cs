using ECommerce.Catalogo.API.Data;
using ECommerce.Catalogo.API.Data.Repository;
using ECommerce.Catalogo.API.Models;
using Microsoft.Extensions.DependencyInjection;

namespace NSE.Catalogo.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<CatalogoContext>();
        }
    }
}