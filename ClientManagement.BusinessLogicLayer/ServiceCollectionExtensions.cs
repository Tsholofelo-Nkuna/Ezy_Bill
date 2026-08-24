using ClientManagement.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Utils.Interfaces;
using Core.Utils.State;
using ClientManagement.BusinessLogicLayer.Models;
using OllamaSharp;
using ClientManagement.BusinessLogicLayer.Helpers;
using Microsoft.Extensions.Configuration;
using ClientManagement.BusinessLogicLayer.Agents;
using ClientManagement.BusinessLogicLayer.Agents.Workflows;



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
                .AddScoped<IClientService, ClientService> ()
                .AddScoped<IInvoiceService, InvoiceService> ()
                .AddScoped<IProductService,ProductService>() //Add interface for the ProductService
                .AddScoped<IInvoiceProductService, InvoiceProductsService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddScoped<IAppFileService, AppFileService>()
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>()
                .AddScoped<AssistantChatApiClient>()
                .AddScoped<BookkeepingAgent>()
                .AddScoped<ImageAnalystAgent>()
                .AddScoped<AppAssistantWorkflowProvider>();
           
            return services;
        }
    }
}
