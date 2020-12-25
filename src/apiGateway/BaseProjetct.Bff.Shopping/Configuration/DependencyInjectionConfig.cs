using System;
using BaseProject.WebAPI.Core.Extensions;
using BaseProject.WebAPI.Core.User;
using BaseProjetct.Bff.Shopping.Extensions;
using BaseProjetct.Bff.Shopping.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace BaseProjetct.Bff.Shopping.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IAspNetUser, AspNetUser>();

            services.AddTransient<HttpClientAuthDelegatingHandler>();

            services.AddHttpClient<ICatalogService, CatalogService>()
                .AddHttpMessageHandler<HttpClientAuthDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.WaitAndTry())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            services.AddHttpClient<IShoppingCartService, ShoppingCartService>()
                .AddHttpMessageHandler<HttpClientAuthDelegatingHandler>()
                .AddPolicyHandler(PollyExtensions.WaitAndTry())
                .AddTransientHttpErrorPolicy(
                    p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));
        }
    }
}
