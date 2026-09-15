using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public class ManagerAgent : AgentBase
    {
        public ManagerAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore)
        {
            Name = "Paul";
        }
    }
}
