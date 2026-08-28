using AutoMapper;
using ClientManagement.BusinessLogicLayer.Agents.Tools;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using CommunityToolkit.VectorData.Qdrant;
using Core.Presentation.Models.DataTransferObjects;
using Core.Utils.Interfaces;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;
using Qdrant.Client;
using Qdrant.Client.Grpc;
namespace ClientManagement.BusinessLogicLayer.Services
{
    public class   AppFileService : GenericService<AppFileDto, AppFileEntity>, IAppFileService
    {
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator;
        protected readonly QdrantClient qdrantClient;
        public AppFileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager, IOptions<AgentOptions> agentOptions) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
            this.agentOptions = agentOptions;
            this.embeddingGenerator = new OllamaApiClient(this.agentOptions.Value.OllamaUrl, this.agentOptions.Value.EmbeddingModel);
            this.qdrantClient = new QdrantClient(new Uri(this.agentOptions.Value.VectorStoreUrl));
        }

        public IEnumerable<(string name, string displayName)> GetVectoreStoreNames() => this.agentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData: VectoreStoreMetaData }).Select(x => (x.VecStoreMetaData!.Name, x.VecStoreMetaData!.DisplayName));

        public async Task<IEnumerable<string>> SearchAsync(string text)
        {
            var input = (await this.embeddingGenerator.GenerateVectorAsync(text)).ToArray();
            var search = new SearchPoints() { 
                CollectionName = this.agentOptions.Value.VectorStoreCollectionName,
                Limit = 5,
                Vector = { input.ToArray() },
                
            };
            var response = await this.qdrantClient.QueryAsync(this.agentOptions.Value.VectorStoreCollectionName, query: input, payloadSelector: true, limit:5);
            
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
                        var eM = await embeddingGenerator.GenerateVectorAsync(chunk);
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
