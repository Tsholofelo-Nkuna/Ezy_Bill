using ClientManagement.Ai.Helpers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents
{
    public class CvAnalystAgent : AgentBase
    {
        public CvAnalystAgent(IOptions<AgentOptions> agentOptions, AssistantChatApiClient chatClient, AppHttpTransportClient appStdIoTransportClient, IVectorStore vectorStore, ILogger<CvAnalystAgent> logger) : base(agentOptions, chatClient, appStdIoTransportClient, vectorStore, logger)
        {
            Name = "Linda";
        }
    }
}
