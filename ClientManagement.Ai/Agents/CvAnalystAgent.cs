using ClientManagement.Ai.Agents.Tools;
using ClientManagement.Ai.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents
{
    public class CvAnalystAgent : AgentBase
    {
        public CvAnalystAgent(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient) : base(agentOptions, ragToolKit, chatClient, appStdIoTransportClient)
        {
            Name = "Linda";
        }
    }
}
