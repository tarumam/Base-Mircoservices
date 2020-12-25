using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace BaseProject.Catalog.API.Application.Events
{
    public class ProductEventHandler : INotificationHandler<ProductAddedEvent>
    {
        public Task Handle(ProductAddedEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
