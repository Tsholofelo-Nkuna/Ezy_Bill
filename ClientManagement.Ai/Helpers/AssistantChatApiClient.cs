using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Helpers
{
    public class AssistantChatApiClient : OllamaApiClient, IChatClient
    {
        public AssistantChatApiClient(IOptions<AgentOptions> apiOptions, ILogger<AssistantChatApiClient> logger) 
            : base(new HttpClient() { BaseAddress = new Uri(apiOptions.Value.OllamaUrl), Timeout = TimeSpan.FromMinutes(3)}, apiOptions.Value.OllamaModel)
           // : base(apiOptions.Value.OllamaUrl, apiOptions.Value.AiAgentMetaData.First(x => x.Type.Equals(AgentType.Master, StringComparison.OrdinalIgnoreCase)).Model)
        {
            Logger = logger;
        }

        public ILogger<AssistantChatApiClient> Logger { get; }
    }
}
