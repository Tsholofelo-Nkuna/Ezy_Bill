using ClientManagement.BusinessLogicLayer.Agents.Interfaces;
using ClientManagement.BusinessLogicLayer.Agents.Tools;
using ClientManagement.BusinessLogicLayer.Helpers;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
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
        protected readonly RagToolKit ragToolKit;
        protected readonly AssistantChatApiClient chatClient;
        public AgentBase(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit, AssistantChatApiClient chatClient) : base(agentOptions)
        {
            this.agentOptions = agentOptions;
            this.ragToolKit = ragToolKit;
            this.chatClient = chatClient;
        }
        public AgentSkillsProvider BaseSkill => new AgentSkillsProvider(Path.Combine(AppContext.BaseDirectory, agentOptions.Value.SkillPath));
        public AIAgent? AgentInstance
        { 
            get
            {
                field ??= Instance(Name);
                return field;
            }
        }
        public string Name { get ; set ; } = string.Empty;

        public virtual AIAgent? Instance(string agentName)
        {
             
            var agent = this.agentOptions.Value.AiAgentMetaData
                 .Where(x => x.Name.Equals(agentName, StringComparison.OrdinalIgnoreCase))
                 .Select(aMetaData =>
                 {
                     var options = new ChatClientAgentOptions()
                     {
                        // AIContextProviders = [BaseSkill],
                         ChatOptions = new()
                         {
                             Instructions = $"Your name is {agentName}. {aMetaData.Instructions}. Always refer to yourself by your name in the case you have to. All your responses should be in plain text. Never mention your internal tools. Always use the `AddInsightToPrompt` tool to add more context to the user's reque, never respond without consulting/making use of the `AddInsightToPrompt` tool. Always be kind, helpful and use a professional tone.",
                             Tools = [AIFunctionFactory.Create(this.ragToolKit.AddInsightToPrompt)],
                             ModelId = aMetaData.Model
                         },
                         Name = aMetaData.Name,
                         Description = aMetaData.Description,
                         
                     };
                     return chatClient.AsAIAgent(options: options);// new ChatClientAgent(chatClient, options);
                 }).FirstOrDefault();
            
            return agent;
        }

       public async Task InspectInputMiddleware(IEnumerable<ChatMessage> messages, AgentSession? session, AgentRunOptions?options, Func<IEnumerable<ChatMessage>, AgentSession?, AgentRunOptions?, CancellationToken, Task> callback, CancellationToken cancellationToken)
    {
        // Example: Log incoming traffic or modify a shared state metric counter
        Console.WriteLine($"Inspecting payload. Total user prompts: {messages.Count()}");
            var userPrompt = messages.LastOrDefault();
            var augmentedTextPrompt = await ragToolKit.AddInsightToPrompt(userPrompt?.Text, Name);
            await callback(messages.Append(new ChatMessage(ChatRole.System, augmentedTextPrompt)), session, options, cancellationToken);
         
       
    }
        public static async Task<string> HandleUserRequest(AIAgent? agent,IList<AIContent> messageContents, IList<ChatMessage> ChatHistory, AgentSession? session = null )
        {
            //var agent = AgentInstance;
            await Task.Yield();
            if(session is null)
            {
                session = await agent.CreateSessionAsync();
            }
           
            var userMessage = new ChatMessage(ChatRole.User, messageContents);
            IList<AIContent> chatContext = [..ChatHistory.SelectMany(x => x.Contents), ..messageContents];
            ChatHistory.Add(userMessage);
            var response = agent.RunStreamingAsync(new ChatMessage(ChatRole.User, chatContext) { }, session);
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
