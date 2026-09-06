using ClientManagement.Ai.Agents.Tools;
using ClientManagement.Ai.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public class ImageAnalystAgent : AgentBase
    {
        public ImageAnalystAgent(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient) : base(agentOptions, ragToolKit, chatClient, appStdIoTransportClient)
        {
            Name = "Paul";
        }
    }
}
