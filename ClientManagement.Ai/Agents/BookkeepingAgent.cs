using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public class BookkeepingAgent : AgentBase
    {
        public BookkeepingAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore, ILogger<BookkeepingAgent> logger) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore, logger)
        {
            Name = "Jimmy";
        }
    }
}
