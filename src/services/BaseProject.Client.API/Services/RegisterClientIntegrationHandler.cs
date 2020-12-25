using System;
using System.Threading;
using System.Threading.Tasks;
using BaseProject.Clients.API.Application.Commands;
using BaseProject.Core.Mediatr;
using BaseProject.Core.Messages.Integration;
using BaseProject.MessageBus;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaseProject.Clients.API.Services
{
    public class RegisterClientIntegrationHandler : BackgroundService
    {
        private readonly IMessageBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisterClientIntegrationHandler(
                            IServiceProvider serviceProvider,
                            IMessageBus bus)
        {
            _serviceProvider = serviceProvider;
            _bus = bus;
        }

        private void SetResponder()
        {
            _bus.RespondAsync<UserRegisteredIntegrationEvent, ResponseMessage>(async request =>
                await RegisterClient(request));

            _bus.AdvancedBus.Connected += OnConnect;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            SetResponder();
            return Task.CompletedTask;
        }

        private void OnConnect(object s, EventArgs e)
        {
            SetResponder();
        }

        private async Task<ResponseMessage> RegisterClient(UserRegisteredIntegrationEvent message)
        {
            var clienteCommand = new AddClientCommand(message.Id, message.Name, message.Email, message.Cpf);
            ValidationResult sucesso;

            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();
                sucesso = await mediator.SendCommand(clienteCommand);
            }

            return new ResponseMessage(sucesso);
        }
    }
}
