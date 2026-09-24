using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using ClientManagement.Utils.Ai;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public class ManagerAgent : AgentBase
    {
        public ManagerAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore, ILogger<ManagerAgent> logger, AgentStoreKeyRegistry storeKeyRegistry) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore, logger, storeKeyRegistry)
        {
            Name = "Paul";
        }
    }
}
