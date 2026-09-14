using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents
{
    public class CvAnalystAgent : AgentBase
    {
        public CvAnalystAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore)
        {
            Name = "Linda";
        }
    }
}
