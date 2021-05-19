using AutoPartsStore.Domain.Auth;
using AutoPartsStore.Persistence;
using AutoPartsStore.Services.Contract;
using AutoPartsStore.Services.Features;
using AutoPartsStore.Services.Implementation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace AutoPartsStore.Services
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Add Services Such a IEmailSender
        /// </summary>
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddScoped<IEmailSender, EmailSender>();
            return services;
        }
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            services.AddScoped<AppIdentityErrorDescriber>();
            services.AddScoped<IAppUserManager,AppUserManager>();
            services.AddIdentity<AppUser, AppRole>(
                op =>
                {
                    op.Password.RequireDigit = false;
                    op.Password.RequireLowercase = false;
                    op.Password.RequireUppercase = false;
                    op.Password.RequiredLength = 8;
                    op.Password.RequireNonAlphanumeric = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddUserManager<AppUserManager>()
                //.AddRoleManager<>
                .AddErrorDescriber<AppIdentityErrorDescriber>()
                .AddDefaultTokenProviders();
            return services;
        }
        
    }
}
