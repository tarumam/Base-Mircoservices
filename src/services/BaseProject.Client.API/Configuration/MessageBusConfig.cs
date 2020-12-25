using BaseProject.Clients.API.Services;
using BaseProject.Core.Utils;
using BaseProject.MessageBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Clients.API.Configuration
{
    public static class MessageBusConfig
    {
        public static void AddMessageBusConfiguration(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMessageBus(configuration.GetMessageQueueConnection("MessageBus"))
                .AddHostedService<RegisterClientIntegrationHandler>();
        }
    }
}
