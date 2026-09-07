using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services;
using ClientManagement.Models.AI;
using Core.Utils.Interfaces;
using Core.Utils.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;


namespace ClientManagement.BusinessLogicLayer
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration config) {
           
            services.Configure<AgentOptions>(options =>
            {
                config.Bind("AI", options);
            });
            services
                .AddAutoMapper(typeof(AutoMapperConfig))
                .AddScoped<IClientService, ClientService>()
                .AddScoped<IInvoiceService, InvoiceService>()
                .AddScoped<IProductService, ProductService>() //Add interface for the ProductService
                .AddScoped<IInvoiceProductService, InvoiceProductsService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddScoped<IAppFileService, AppFileService>()
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>();
                
           
            return services;
        }
    }
}
