using EasyNetQ;
using ECommerce.Cliente.API.Application.Commands;
using ECommerce.Core.Mediator;
using ECommerce.Core.Messages.Integration;
using FluentValidation.Results;
using MessageBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Cliente.API.Services
{
    //Processa em Background dentro do pipeline da própria aplicação
    public class RegistroClienteIntegrationHandler : BackgroundService
    {
        private IServiceProvider _serviceProvider;
        private IMessageBus _bus;

        public RegistroClienteIntegrationHandler(IServiceProvider serviceProvider,
            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(async request =>
                await RegistrarCliente(request));

            //Esse evento é disparado quando rabbit estiver conectado com a aplicação
            _bus.AdvancedBus.Connected += OnConnect;
        }

        //É chamado quando a minha aplicação subir
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        //É como se eu estiver renovando minha conexão
        private void OnConnect(object s, EventArgs e)
        {
            //Vou registrar novamente que eu to esperando alguma coisa
            SetResponder();
        }

        private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistradoIntegrationEvent message)
        {
            var clienteCommand = new RegistrarClienteCommand(message.Id, message.Nome, message.Email, message.Cpf);
            ValidationResult retorno;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                retorno = await mediator.EnviarComando(clienteCommand);
            }
            return new ResponseMessage(retorno);
        }
    }
}
