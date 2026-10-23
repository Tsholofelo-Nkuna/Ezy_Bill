using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ClientManagement.Ai.Helpers
{
    public class AssistantChatApiClient : OllamaApiClient, IChatClient
    {
        public delegate void ChatResponseHandler((string responseToken, string reasoningToken) payload);
        public event ChatResponseHandler? ChatResponseTokenReceived;
        protected  AgentOptions AOptions { get; }
        public AssistantChatApiClient(IOptions<AgentOptions> apiOptions, ILogger<AssistantChatApiClient> logger) 
            : base(new HttpClient() { BaseAddress = new Uri(apiOptions.Value.OllamaUrl), Timeout = TimeSpan.FromMinutes(3)}, apiOptions.Value.OllamaModel)
           // : base(apiOptions.Value.OllamaUrl, apiOptions.Value.AiAgentMetaData.First(x => x.Type.Equals(AgentType.Master, StringComparison.OrdinalIgnoreCase)).Model)
        {
            Logger = logger;
            AOptions = apiOptions.Value;
        }

        public ILogger<AssistantChatApiClient> Logger { get; }
        public async Task<string> GenerateResponseAsync(string prompt)
        {

            var responseStream = GenerateAsync(new()
            {
                Model = AOptions.OllamaModel,
                Prompt = prompt
            });
            var llmResponse = string.Empty;
            if(responseStream is not null)
            {
                await foreach(var responseItem in responseStream)
                {
                    llmResponse += responseItem?.Response;
                    ChatResponseTokenReceived?.Invoke((responseItem?.Response ?? string.Empty, responseItem?.Thinking ?? string.Empty));
                }
            }

            return llmResponse;
        }

        public async Task<SystemOneResponse?> SystemOneChoiceQuestion(string state, Dictionary<string, string> criteria)
        {
            SystemOneResponse? ans = null;
            Logger.LogInformation($"sending systemone choice question to {AOptions.SystemOneModel}");
            ans = await SystemOneAsync(new()
            {
                Model = AOptions.SystemOneModel,
                State = state,
                Questions = {
                     ["label"] = new SystemOneChoiceQuestion()
                     {
                         Criteria = criteria,
                         Instructions = "Which profession is this in alignment with?"
                     }
                    }
            });
            Logger.LogInformation($"systemone choice answer: {JsonSerializer.Serialize(ans)}");
            return ans;
        }

        ~ AssistantChatApiClient() {
            ChatResponseTokenReceived = null;
        }
    }
}
