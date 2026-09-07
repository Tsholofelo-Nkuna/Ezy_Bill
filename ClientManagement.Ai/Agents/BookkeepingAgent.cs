using ClientManagement.Ai.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;



namespace ClientManagement.Ai.Agents
{
    public class BookkeepingAgent : AgentBase
    {
        public BookkeepingAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient) : base(agentOptions, chatClient, appStdIoTransportClient)
        {
            Name = "Jimmy";
        }
    }
}
