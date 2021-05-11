using ECommerce.Cliente.API.Application.Commands;
using ECommerce.Cliente.API.Application.Events;
using ECommerce.Cliente.API.Data;
using ECommerce.Cliente.API.Data.Repository;
using ECommerce.Cliente.API.Models;
using ECommerce.Cliente.API.Services;
using ECommerce.Clientes.API.Application.Commands;
using ECommerce.Core.Mediator;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Cliente.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<RegistrarClienteCommand, ValidationResult>, ClienteCommandHandler>();

            services.AddScoped<INotificationHandler<ClienteRegistradoEvent>, ClienteEventHandler>();

            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<ClientesContext>();

            //Registro de classes que rodam em background (herdam de BackgroundService)
            //services.AddHostedService<RegistroClienteIntegrationHandler>();



        }
    }
}