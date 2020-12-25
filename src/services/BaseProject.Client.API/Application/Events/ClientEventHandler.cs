using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BaseProject.Clients.API.Application.Events
{
    public class ClientEventHandler : INotificationHandler<ClientAddedEvent>
    {
        public Task Handle(ClientAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
