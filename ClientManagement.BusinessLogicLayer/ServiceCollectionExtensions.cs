using ClientManagement.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Utils.Interfaces;
using Core.Utils.State;
using ClientManagement.BusinessLogicLayer.Models;
using OllamaSharp;
using ClientManagement.BusinessLogicLayer.Helpers;



namespace ClientManagement.BusinessLogicLayer
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddBusinessServices(this IServiceCollection services) {
            services
                .AddAutoMapper(typeof(AutoMapperConfig))
                .AddScoped<IClientService, ClientService> ()
                .AddScoped<IInvoiceService, InvoiceService> ()
                .AddScoped<IProductService,ProductService>() //Add interface for the ProductService
                .AddScoped<IInvoiceProductService, InvoiceProductsService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>()
                .AddScoped<IOllamaApiClient, AssistantChatApiClient>()
                .AddScoped<IChatAssistantService, ChatAssistantService>();
           
            return services;
        }
    }
}
