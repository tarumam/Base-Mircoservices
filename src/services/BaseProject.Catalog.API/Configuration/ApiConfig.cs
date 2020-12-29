using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BaseProject.Catalog.Infra;
using Microsoft.EntityFrameworkCore;
using System;
using BaseProject.WebAPI.Core.Extensions;

namespace BaseProject.Catalog.API.Configuration
{
    public static class ApiConfig
    {

        public static string GetConnectionString(IConfiguration configuration)
        {
            var conStr = configuration.GetConnectionString("DefaultConnection") ?? HerokuConnection.GetHerokuConnection();
            return conStr ?? throw new ArgumentNullException("Não foi possível estabelecer conexão com o banco de dados");
        }

        public static void AddApiConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CatalogContext>(options =>
                options.UseNpgsql(GetConnectionString(configuration)));

            services.AddControllers();

            services.AddCors(options =>
            {
                options.AddPolicy("Total",
                    builder =>
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());
            });
        }

        public static void UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("Total");

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
