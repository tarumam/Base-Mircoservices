using System;
using BaseProject.Identity.API.Data;
using BaseProject.Identity.API.Extensions;
using BaseProject.WebAPI.Core.Extensions;
using BaseProject.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BaseProject.Identity.API.Configuration
{
    public static class IdentityConfig
    {
        public static string GetConnectionString(IConfiguration configuration)
        {
            var conStr = configuration.GetConnectionString("DefaultConnection") ?? HerokuConnection.GetHerokuConnection();
            return conStr ?? throw new ArgumentNullException("Não foi possível estabelecer conexão com o banco de dados");
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(GetConnectionString(configuration)));

            services.AddDefaultIdentity<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddErrorDescriber<IdentityMessagesPtBr>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddJwtConfiguration(configuration);

            return services;
        }
    }
}
