using BaseProject.Catalog.API.Application.Commands;
using BaseProject.Catalog.API.Application.Events;
using BaseProject.Catalog.Domain;
using BaseProject.Catalog.Infra;
using BaseProject.Catalog.Infra.Data.Repository;
using BaseProject.Core.Mediatr;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Catalog.API.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<AddProductCommand, ValidationResult>, ProductCommandHandler>();
            services.AddScoped<IRequestHandler<AddPriceCommand, ValidationResult>, ProductCommandHandler>();

            services.AddScoped<INotificationHandler<ProductAddedEvent>, ProductEventHandler>();

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<CatalogContext>();
        }
    }
}
