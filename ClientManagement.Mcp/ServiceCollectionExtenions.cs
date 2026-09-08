using ClientManagement.BusinessLogicLayer.Helpers;
using ClientManagement.BusinessLogicLayer.Helpers.Interface;
using ClientManagement.Mcp.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;

namespace ClientManagement.Mcp
{
    public static class ServiceCollectionExtenions
    {
        extension(IServiceCollection services) {

            public IServiceCollection AddMcpServices(IConfiguration configuration) {

                services.Configure<AgentOptions>(options =>
                {
                    configuration.Bind("AI", options);
                });
                services.AddScoped<McpOllamaApiClient>()
                        .AddScoped<IEmbeddingGenerator<string, Embedding<float>>, McpOllamaApiClient>()
                        .AddScoped<IVectorStore, VectorStore>();
                return services;
            }
        }
    }
}
