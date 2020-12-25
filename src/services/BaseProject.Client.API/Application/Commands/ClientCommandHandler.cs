using System.Threading;
using System.Threading.Tasks;
using BaseProject.Clients.API.Application.Events;
using BaseProject.Clients.API.Models;
using BaseProject.Core.Messages;
using FluentValidation.Results;
using MediatR;

namespace BaseProject.Clients.API.Application.Commands
{
    public class ClientCommandHandler : CommandHandler,
        IRequestHandler<AddClientCommand, ValidationResult>
    {
        private readonly IClientRepository _clienteRepository;

        public ClientCommandHandler(IClientRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public async Task<ValidationResult> Handle(AddClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var cliente = new Client(message.Id, message.Name, message.Email, message.Cpf);

            var clienteExistente = await _clienteRepository.GetByCPF(cliente.Cpf.Numero);

            if (clienteExistente != null)
            {
                AdicionarErro("Este CPF já está em uso.");
                return ValidationResult;
            }

            _clienteRepository.Add(cliente);

            cliente.AddEvent(new ClientAddedEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistirDados(_clienteRepository.UnitOfWork);
        }
    }
}
