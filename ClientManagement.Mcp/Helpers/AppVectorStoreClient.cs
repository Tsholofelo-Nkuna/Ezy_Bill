
using ClientManagement.Models.AI;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.DataAccessLayer;
using ClientManagement.Models.DataTransferObjects;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using OllamaSharp;
using ClientManagemet.Models;

namespace ClientManagement.Mcp.Helpers
{
    public class   AppVectorStoreClient : IVectorStore
    {
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly OllamaApiClient chatClient;
        protected readonly IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator;
        protected readonly QdrantClient qdrantClient;
        public AppVectorStoreClient(WebDbContext dbContext, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager, IOptions<AgentOptions> agentOptions, OllamaApiClient chatClient) 
        {
            this.agentOptions = agentOptions;
            this.chatClient = chatClient;
           // this.embeddingGenerator = new OllamaApiClient(this.agentOptions.Value.OllamaUrl, this.agentOptions.Value.EmbeddingModel);
            this.qdrantClient = new QdrantClient(new Uri(this.agentOptions.Value.VectorStoreUrl));
        }

        public IEnumerable<(string name, string displayName)> GetVectoreStoreNames() => this.agentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData: VectoreStoreMetaData }).Select(x => (x.VecStoreMetaData!.Name, x.VecStoreMetaData!.DisplayName));

        public async Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text)
        {
            var input = (await this.chatClient.GenerateVectorAsync(text, new() { ModelId = this.agentOptions.Value.EmbeddingModel})).ToArray(); 
            //var search = new SearchPoints() { 
            //    CollectionName = this.agentOptions.Value.VectorStoreCollectionName,
            //    Limit = 5,
            //    Vector = { input.ToArray() },
                
            //};
            var response = await this.qdrantClient.QueryAsync(vectorStoreCollectionName, query: input, payloadSelector: true, limit:5);
            
            return response.Select(x =>
            {
                x.Payload.TryGetValue("text", out var text);
                return text.ToString();
            });
        }

        public async Task<bool> UpSert(string vectoreStoreCollectionName, List<AppFileDto> payload)
        {
            var collectionExists = await this.qdrantClient.CollectionExistsAsync(vectoreStoreCollectionName);
            if (!qdrantClient.CollectionExistsAsync(vectoreStoreCollectionName).Result)
            {
                await qdrantClient.CreateCollectionAsync(vectoreStoreCollectionName, new VectorParams()
                {
                    Distance = Distance.Cosine,
                    Size = 1024,
                });

            }

            foreach (var file in payload)
            {
                if (file.MimeType.Contains("pdf", StringComparison.OrdinalIgnoreCase))
                {


                    await foreach (var chunk in DocumentToolKit.ReadPdf(file.Contents))
                    {
                        var eM = await this.chatClient.GenerateVectorAsync(chunk, new() { ModelId = this.agentOptions.Value.EmbeddingModel});
                        var point = new PointStruct()
                        {
                            Id = Guid.NewGuid(),
                            Vectors = eM.ToArray(),
                            Payload =
                            {
                                ["text"] = chunk
                            }
                        };

                        await qdrantClient.UpsertAsync(vectoreStoreCollectionName, [point]);

                    }
                }
            }
            return true;
        }
    }
}
