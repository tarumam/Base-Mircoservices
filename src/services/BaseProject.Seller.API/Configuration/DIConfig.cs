using BaseProject.Core.Mediatr;
using BaseProject.Seller.API.Application.Commands;
using BaseProject.Seller.API.Data;
using BaseProject.Seller.API.Data.Repository;
using FluentValidation.Results;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Seller.API.Configuration
{
    public static class DIConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<IRequestHandler<AddSellerCommand, ValidationResult>, SellerCommandHandler>();
            //services.AddScoped<IRequestHandler<UpdateSellerCommand, ValidationResult>, SellerCommandHandler>();

            //services.AddScoped<INotificationHandler<SellerAddedEvent>, SellerEventHandler>();
            //services.AddScoped<INotificationHandler<SellerUpdatedEvent>, SellerEventHandler>();

            services.AddScoped<ISellerRepository, SellerRepository>();
            services.AddScoped<SellerContext>();
        }
    }
}
