using BaseProject.Clients.API.Application.Commands;
using BaseProject.Clients.API.Application.Events;
using BaseProject.Clients.API.Data;
using BaseProject.Clients.API.Data.Repository;
using BaseProject.Clients.API.Models;
using BaseProject.Core.Mediatr;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Clients.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<AddClientCommand, ValidationResult>, ClientCommandHandler>();

            services.AddScoped<INotificationHandler<ClientAddedEvent>, ClientEventHandler>();

            services.AddScoped<IClientRepository, ClientRepository>();
            services.AddScoped<ClientsContext>();
        }
    }
}
