using System;
using BaseProject.Catalog.Infra;
using BaseProject.Catalog.Infra.Data;
using BaseProject.WebAPI.Core.Extensions;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

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

            services.AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.Formatting = Formatting.Indented;
                });

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

            using (var contextScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("****************Criação do contexto CATALOG****************");
                var context = contextScope.ServiceProvider.GetRequiredService<CatalogContext>();
                DBInitializer.Initialize(context);
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
