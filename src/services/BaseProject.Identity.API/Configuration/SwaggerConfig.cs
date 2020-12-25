using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BaseProject.Identity.API.Configuration
{
    public static class SwaggerConfig
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                {
                    Title = "Base Project",
                    Description = "Base project for .NetCore API",
                    Contact = new OpenApiContact() { Name = "Taruma", Email = "On definition" },
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });
            });
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/V1/swagger.json", "V1");
            });
            return app;
        }
    }
}
