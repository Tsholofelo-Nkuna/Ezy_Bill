
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Configuration;
using ClientManagement.Models.AI;
using ClientManagement.Ai.Helpers;
using ClientManagement.Ai.Agents;
using ClientManagement.Ai.Agents.Workflows;
using Microsoft.Extensions.AI;


namespace ClientManagement.Ai
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddAiAgents(this IServiceCollection services, IConfiguration config) {
            //services
            //    .AddAutoMapper(typeof(AutoMapperConfig))
            //    .AddScoped<IClientService, ClientService> ()
            //    .AddScoped<IInvoiceService, InvoiceService> ()
            //    .AddScoped<IProductService,ProductService>() //Add interface for the ProductService
            //    .AddScoped<IInvoiceProductService, InvoiceProductsService>()
            //    .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
            //    .AddScoped<IProfileService, ProfileService>()
            //    .AddScoped<IUserProfileService, UserProfileService>()
            //    .AddScoped<IVectorStore, AppVectorStoreClient>()
            //    .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>()
            //    .AddScoped<AssistantChatApiClient>()
            //    .AddScoped<BookkeepingAgent>()
            //    .AddScoped<manageAgent>()
            //    .AddScoped<CvAnalystAgent>()
            //    .AddScoped<AppAssistantWorkflowProvider>()
            //    .AddScoped<RagToolKit>()
            //    .AddScoped<AppHttpTransportClient>();

                services
                .AddScoped<AssistantChatApiClient>()
                .AddScoped<BookkeepingAgent>()
                .AddScoped<ManagerAgent>()
                .AddScoped<CvAnalystAgent>()
                .AddScoped<AppAssistantWorkflowProvider>()
                .AddScoped<AppHttpTransportClient>()
                .AddScoped<IEmbeddingGenerator<string,Embedding<float>>, AssistantChatApiClient>(); 

            return services;
        }
    }
}
