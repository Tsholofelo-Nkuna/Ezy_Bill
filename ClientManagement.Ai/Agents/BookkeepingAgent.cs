using ClientManagement.Ai.Agents.Tools;
using ClientManagement.Ai.Helpers;
using ClientManagement.Ai.Models;

using Microsoft.Extensions.Options;



namespace ClientManagement.Ai.Agents
{
    public class BookkeepingAgent : AgentBase
    {
        public BookkeepingAgent(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit, AssistantChatApiClient chatClient, AppStdIoTransportClient appStdIoTransportClient) : base(agentOptions, ragToolKit, chatClient, appStdIoTransportClient)
        {
            Name = "Jimmy";
        }
    }
}
