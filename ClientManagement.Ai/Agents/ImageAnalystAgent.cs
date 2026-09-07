using ClientManagement.Ai.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public class ImageAnalystAgent : AgentBase
    {
        public ImageAnalystAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient) : base(agentOptions, chatClient, appStdIoTransportClient)
        {
            Name = "Paul";
        }
    }
}
