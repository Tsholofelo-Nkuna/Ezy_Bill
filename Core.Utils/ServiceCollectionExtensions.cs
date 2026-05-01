using Core.Utils.Mail;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;


namespace Core.Utils
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUtilServices(this IServiceCollection services, IConfiguration config) 
        {
            var emailSettings = JsonSerializer.Deserialize < EmailSettings > (config.GetSection("EmailSettings").ToJsonString());
            services.Configure<EmailSettings>(opt =>opt = emailSettings ?? new());
            services.AddScoped<MailSender>();
            return services;
        }
    }
}
