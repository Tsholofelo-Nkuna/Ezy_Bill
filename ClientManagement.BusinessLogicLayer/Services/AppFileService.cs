using AutoMapper;
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

        private readonly VectorStore _vStore;
        public AppFileService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager, IAppStateManager<ApplicationState> appStateManager, IOptions<AgentOptions> agentOptions, IEmbeddingGenerator<string, Embedding<float>> chatClient) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
            this.agentOptions = agentOptions;
            this.chatClient = chatClient;
            this.qdrantClient = new QdrantClient(new Uri(this.agentOptions.Value.VectorStoreUrl));
           
        }
    }
}
