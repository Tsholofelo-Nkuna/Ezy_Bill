using ClientManagement.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Utils.Interfaces;
using Core.Utils.State;
using ClientManagement.BusinessLogicLayer.Models;



namespace ClientManagement.BusinessLogicLayer
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddBusinessServices(this IServiceCollection services) {
           return services
                .AddAutoMapper(typeof(AutoMapperConfig))
                .AddScoped<IClientService, ClientService> ()
                .AddScoped<IInvoiceService, InvoiceService> ()
                .AddScoped<ProductService>() //Add interface for the ProductService
                .AddScoped<IInvoiceProductService, InvoiceProductsService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>(); ;
                

          
        }
    }
}
