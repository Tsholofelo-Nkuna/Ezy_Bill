using ClientManagement.BusinessLogicLayer.Agents.Interfaces;
using ClientManagement.BusinessLogicLayer.Helpers;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents
{
    public abstract class AgentBase : AssistantChatApiClient, IAgentBase
    {
        public List<ChatMessage> ChatHistory = [];
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly IOptions<OllamaOptions> options;
        public AgentBase(IOptions<AgentOptions> agentOptions, IOptions<OllamaOptions> options) : base(options)
        {
            this.agentOptions = agentOptions;
            this.options = options;
        }
        public AgentSkillsProvider BaseSkill => new AgentSkillsProvider(Path.Combine(AppContext.BaseDirectory, agentOptions.Value.SkillPath));

        public ChatClientAgent? AgentInstance
        { 
            get ; 
            set => field ??= Instance(Name);
            
        }
        public string Name { get ; set ; } = string.Empty;

        public virtual ChatClientAgent Instance(string agentName)
        {
           var agentMetaData = this.agentOptions.Value.AiAgentMetaData.FirstOrDefault(x => x.Type == AgentType.Master);
           var wokerAgents = this.agentOptions.Value.AiAgentMetaData
                .Where(x => x.Type == AgentType.Worker)
                .Select(aMetaData =>
                {
                    var options = new ChatClientAgentOptions()
                    {
                        AIContextProviders = [BaseSkill],
                        ChatOptions = new()
                        {
                            Instructions = aMetaData.Instructions
                        },
                        Name = aMetaData.Name
                    };
                    return new ChatClientAgent(new OllamaApiClient(this.options.Value.Url, aMetaData.Model));
                });
            var agentOptions = new ChatClientAgentOptions()
            {
                AIContextProviders = [BaseSkill],
                ChatOptions = new()
                {
                    Instructions = agentMetaData?.Instructions,
                    Tools = [..wokerAgents.Select(x => x.AsAIFunction())]
                },
            };
            return this.AsAIAgent(agentOptions);
        }
        public async Task<string> HandleUserRequest(string agentName, IList<AIContent> messageContents)
        {
            var agent = Instance(agentName);
            await Task.Yield();

            var session = await agent.CreateSessionAsync();
            var userMessage = new ChatMessage(ChatRole.User, messageContents);
            IList<AIContent> chatContext = [.. this.ChatHistory.Select(x => new TextContent(x.Text)), new TextContent(userMessage.Text)];
            this.ChatHistory.Add(userMessage);
            var response = agent.RunStreamingAsync(new ChatMessage(ChatRole.User, chatContext), session);
            var agentResponse = string.Empty;
            await foreach (var item in response)
            {

                var toolApprovalRequestContent = item.Contents
                    .OfType<ToolApprovalRequestContent>().FirstOrDefault();
                if (toolApprovalRequestContent is not null)
                {
                    var toolResponse = toolApprovalRequestContent.CreateResponse(true);
                    var wrapped = new ChatMessage(ChatRole.User, [toolResponse]);
                    await foreach (var approvedItem in agent.RunStreamingAsync(wrapped, session))
                    {
                        agentResponse += approvedItem.Text;
                    }
                }
                else
                {
                    agentResponse += item.Text;
                }

            }
            this.ChatHistory.Add(new ChatMessage(ChatRole.Assistant, [new TextContent(agentResponse)]));
            return agentResponse;
        }
    }
}
