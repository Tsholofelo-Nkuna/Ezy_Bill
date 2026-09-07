using AutoMapper;
using ClientManagement.BusinessLogicLayer.Helpers;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Models.AI;
using ClientManagement.Models.DataTransferObjects;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

using Qdrant.Client;
using Qdrant.Client.Grpc;


namespace ClientManagement.BusinessLogicLayer.Services
{
    public class AppFileService : GenericService<AppFileDto, AppFileEntity>, IAppFileService
    {
        private readonly IOptions<AgentOptions> agentOptions;
        private readonly IEmbeddingGenerator<string, Embedding<float>> chatClient;
        private readonly QdrantClient qdrantClient;

        public AppFileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, IAppStateManager<ApplicationState> appStateManager, IOptions<AgentOptions> agentOptions, IEmbeddingGenerator<string, Embedding<float>> chatClient) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
            this.agentOptions = agentOptions;
            this.chatClient = chatClient;
            this.qdrantClient = new QdrantClient(new Uri(this.agentOptions.Value.VectorStoreUrl));
        }


        public IEnumerable<(string name, string displayName)> GetVectoreStoreNames() => this.agentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData: VectoreStoreMetaData }).Select(x => (x.VecStoreMetaData!.Name, x.VecStoreMetaData!.DisplayName));

        public async Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text)
        {
            var input = (await this.chatClient.GenerateVectorAsync(text, new() { ModelId = this.agentOptions.Value.EmbeddingModel })).ToArray();
            //var search = new SearchPoints() { 
            //    CollectionName = this.agentOptions.Value.VectorStoreCollectionName,
            //    Limit = 5,
            //    Vector = { input.ToArray() },

            //};
            var response = await this.qdrantClient.QueryAsync(vectorStoreCollectionName, query: input, payloadSelector: true, limit: 5);

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
                        var eM = await this.chatClient.GenerateVectorAsync(chunk, new() { ModelId = this.agentOptions.Value.EmbeddingModel });
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
