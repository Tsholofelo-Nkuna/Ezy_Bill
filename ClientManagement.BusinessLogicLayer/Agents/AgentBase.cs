using ClientManagement.BusinessLogicLayer.Agents.Interfaces;
using ClientManagement.BusinessLogicLayer.Helpers;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Google.Protobuf.WellKnownTypes;
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
        
        public AgentBase(IOptions<AgentOptions> agentOptions) : base(agentOptions)
        {
            this.agentOptions = agentOptions;
        }
        public AgentSkillsProvider BaseSkill => new AgentSkillsProvider(Path.Combine(AppContext.BaseDirectory, agentOptions.Value.SkillPath));

        public ChatClientAgent? AgentInstance
        { 
            get
            {
                field ??= Instance(Name);
                return field;
            }
        }
        public string Name { get ; set ; } = string.Empty;

        public virtual ChatClientAgent? Instance(string agentName)
        {
             
            var agent = this.agentOptions.Value.AiAgentMetaData
                 .Where(x => x.Name.Equals(agentName, StringComparison.OrdinalIgnoreCase))
                 .Select(aMetaData =>
                 {
                     var options = new ChatClientAgentOptions()
                     {
                         //AIContextProviders = [BaseSkill],
                         ChatOptions = new()
                         {
                             Instructions = aMetaData.Instructions
                         },
                         Name = aMetaData.Name,
                         Description = aMetaData.Description,

                     };
                     return new ChatClientAgent(new OllamaApiClient(this.agentOptions.Value.OllamaUrl, aMetaData.Model), options);
                 }).FirstOrDefault();
           
            return agent;
        }
        public static async Task<string> HandleUserRequest(AIAgent? agent,IList<AIContent> messageContents, IList<ChatMessage> ChatHistory)
        {
            //var agent = AgentInstance;
            await Task.Yield();

            var session = await agent.CreateSessionAsync();
            var userMessage = new ChatMessage(ChatRole.User, messageContents);
            IList<AIContent> chatContext = [..ChatHistory.SelectMany(x => x.Contents), ..messageContents];
            ChatHistory.Add(userMessage);
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
                await Task.Yield();
            }
            ChatHistory.Add(new ChatMessage(ChatRole.Assistant, [new TextContent(agentResponse)]));
            return agentResponse;
        }

        public ValueTask<AgentSession> CreateSessionAsync()
        {
            return AgentInstance!.CreateSessionAsync();
        }
    }
}
