using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Crypto;



namespace ClientManagement.Ai.Agents
{
    public class BookkeepingAgent : AgentBase
    {
        public BookkeepingAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore)
        {
            Name = "Jimmy";
        }
    }
}
