using BaseProject.Catalog.API.Services;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Catalog.API.Configuration
{
    public static class BackgroundServiceConfig
    {
        public static void AddBackgroundServiceConfiguration(this IServiceCollection services)
        {
            services.AddHostedService<GetProductInfoService>();
        }
    }
}
