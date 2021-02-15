using System;
using BaseProject.Seller.API.Data;
using BaseProject.WebAPI.Core.Data;
using BaseProject.WebAPI.Core.Extensions;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BaseProject.Seller.API.Configuration
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
            services.AddDbContext<SellerContext>(options =>
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

            using (var contextScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("****************Criação do contexto Sellers****************");
                var context = contextScope.ServiceProvider.GetRequiredService<SellerContext>();
                new DBInitializer<SellerContext>().Initialize(context);
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
