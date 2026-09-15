using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using ClientManagement.Utils.Interfaces;
using ClientManagement.Utils.State;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace ClientManagement.BusinessLogicLayer
{
    public static class ServiceCollectionExtensions
    {
       public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration config) {
          
            services
                .AddAutoMapper(typeof(AutoMapperConfig))
                .AddScoped<IClientService, ClientService>()
                .AddScoped<IInvoiceService, InvoiceService>()
                .AddScoped<IProductService, ProductService>() //Add interface for the ProductService
                .AddScoped<IInvoiceProductService, InvoiceProductsService>()
                .AddScoped<IInvoicePaymentService, InvoicePaymentService>()
                .AddScoped<IProfileService, ProfileService>()
                .AddScoped<IUserProfileService, UserProfileService>()
                .AddScoped<IAppFileService, AppFileService>() //required an IEmbeddingGenerator<string, Embedding<float>> which is provided by `AddAiAgents` extension method found in the ClientManagement.AI project
                .AddScoped<IAppStateManager<ApplicationState>, AppStateManager<ApplicationState>>()
                .AddScoped<IVectorStore, VectorStore>();
                
           
            return services;
        }
    }
}
