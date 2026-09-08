using ClientManagement.BusinessLogicLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using ClientManagement.Models.DataTransferObjects;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Qdrant.Client;
using Qdrant.Client.Grpc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Helpers
{
    public class VectorStore : IVectorStore
    {
        private readonly IOptions<AgentOptions> agentOptions;
        private readonly IEmbeddingGenerator<string, Embedding<float>> chatClient;
        private QdrantClient qdrantClient;

        public VectorStore(IOptions<AgentOptions> agentOptions, IEmbeddingGenerator<string, Embedding<float>> chatClient)
        {
            this.agentOptions = agentOptions;
            this.chatClient = chatClient;
            this.qdrantClient = new QdrantClient(new Uri(this.agentOptions.Value.VectorStoreUrl));
            this.agentOptions = agentOptions;
        }
        public IEnumerable<(string name, string displayName, string agentName)> GetVectoreStoreNames() => this.agentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData: VectoreStoreMetaData }).Select(x => (x.VecStoreMetaData!.Name, x.VecStoreMetaData!.DisplayName, x.Name));

        public async Task<IEnumerable<string>> SearchAsync(string vectorStoreCollectionName, string text, string agentName)
        {
            if (!(await this.qdrantClient.CollectionExistsAsync(vectorStoreCollectionName)))
            {
                return [];
            }
            var input = (await this.chatClient.GenerateVectorAsync(text, new() { ModelId = this.agentOptions.Value.EmbeddingModel })).ToArray();
            
            var response = await this.qdrantClient.QueryAsync(vectorStoreCollectionName, query: input, payloadSelector: true, limit: 5, filter: Conditions.MatchText("agent_name", agentName.ToLower()) );

            return response.Select(x =>
            {
                x.Payload.TryGetValue("text", out var text);
                return text.ToString();
            });
        }

        public async Task<bool> UpSert(string vectoreStoreCollectionName, List<AppFileDto> payload, string agentName)
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
                                ["text"] = chunk,
                                ["agent_name"] = agentName.ToLower(),
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
