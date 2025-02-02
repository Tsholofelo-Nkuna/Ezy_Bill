using Core.Utils.Mail;
using Core.Utils.State;
using Core.Utils.State.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEmailSender(this IServiceCollection services, IConfiguration config) 
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddScoped<IEmailSender<IdentityUser>, MailSender>();
            return services;
        }

        public static IServiceCollection AddAppStateService<TState>(this IServiceCollection services) where TState : new()
        {
            services.AddSingleton<IAppStateService, AppStateService<TState>>();
            return services;
        }
     }
}
