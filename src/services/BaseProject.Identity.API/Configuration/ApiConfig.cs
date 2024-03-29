﻿using System;
using BaseProject.Identity.API.Data;
using BaseProject.WebAPI.Core.Data;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaseProject.Identity.API.Configuration
{
    public static class ApiConfig
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            using (var contextScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("****************Criação do contexto CATALOG****************");
                var context = contextScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                new DBInitializer<ApplicationDbContext>().Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
