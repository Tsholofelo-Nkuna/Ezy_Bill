using ClientManagement.BusinessLogicLayer.Services;
using Microsoft.Extensions.DependencyInjection;
using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Utils.Interfaces;
using Core.Utils.State;
using ClientManagement.BusinessLogicLayer.Models;
using Microsoft.Extensions.Configuration;
using ClientManagement.Ai.Models;
using ClientManagement.Ai.Helpers;
using ClientManagement.Ai.Agents;
using ClientManagement.Ai.Agents.Workflows;
using ClientManagement.Ai.Agents.Tools;
using ClientManagement.BusinessLogicLayer;
using ClientManagement.Ai.Interfaces;

namespace ClientManagement.Ai
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddAiBusinessServices(this IServiceCollection services, IConfiguration config) {
           
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
                .AddScoped<IVectorStore, AppFileService>()
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>()
                .AddScoped<AssistantChatApiClient>()
                .AddScoped<BookkeepingAgent>()
                .AddScoped<ImageAnalystAgent>()
                .AddScoped<CvAnalystAgent>()
                .AddScoped<AppAssistantWorkflowProvider>()
                .AddScoped<RagToolKit>()
                .AddScoped<AppStdIoTransportClient>();
           
            return services;
        }
    }
}
