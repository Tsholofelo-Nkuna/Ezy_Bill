using ClientManagement.Ai.Agents.Interfaces;
using ClientManagement.Ai.Agents.Tools;
using ClientManagement.Ai.Helpers;
using ClientManagement.Ai.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;

namespace ClientManagement.Ai.Agents
{
    public abstract class AgentBase : AssistantChatApiClient, IAgentBase
    {
        public List<ChatMessage> ChatHistory = [];
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly RagToolKit ragToolKit;
        protected readonly AssistantChatApiClient chatClient;
        private readonly AppStdIoTransportClient stdIoTransportClient;

        public AgentBase(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit, AssistantChatApiClient chatClient, AppStdIoTransportClient stdIoTransportClient) : base(agentOptions)
        {
            this.agentOptions = agentOptions;
            this.ragToolKit = ragToolKit;
            this.chatClient = chatClient;
            this.stdIoTransportClient = stdIoTransportClient;
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
                         AIContextProviders = [BaseSkill],
                         ChatOptions = new()
                         {
                             Instructions = $"Your name is {agentName}. {aMetaData.Instructions}. Always refer to yourself by your name in the case you have to. All your responses should be in plain text. Never mention your internal tools. Always be kind, helpful and use a professional tone.",
                             Tools = [AIFunctionFactory.Create(this.ragToolKit.AddInsightToPrompt), AIFunctionFactory.Create(DocumentToolKit.DocumentClipper), ..this.stdIoTransportClient.Tools],
                             ModelId = string.IsNullOrWhiteSpace(aMetaData.Model) ? this.agentOptions.Value.OllamaModel : aMetaData.Model,
                         },
                         Name = aMetaData.Name,
                         Description = aMetaData.Description,
                         
                     };
                     return chatClient.AsAIAgent(options: options);//.AsBuilder().Use(sharedFunc: InspectInputMiddleware).Build();// new ChatClientAgent(chatClient, options);
                 }).FirstOrDefault();
            
            return agent;
        }

       public async Task InspectInputMiddleware(IEnumerable<ChatMessage> messages, AgentSession? session, AgentRunOptions?options, Func<IEnumerable<ChatMessage>, AgentSession?, AgentRunOptions?, CancellationToken, Task> next, CancellationToken cancellationToken)
    {
        // Example: Log incoming traffic or modify a shared state metric counter
            Console.WriteLine($"Inspecting payload. Total user prompts: {messages.Count()}");
            var userPrompt = messages.LastOrDefault();
            var dataContent = userPrompt?.Contents?.OfType<DataContent>() ?? [];
            
            var augmentedTextPrompt = await ragToolKit.AddInsightToPrompt(userPrompt?.Text, Name);
            var augmentedMessage = new ChatMessage(ChatRole.System, [new TextContent(augmentedTextPrompt),..dataContent]);
            var augmentedMessages = messages.Select((message, index) =>
            {
                return index == messages.Count() - 1 ? augmentedMessage : message;
            });
            await next(augmentedMessages, session, options, cancellationToken);
         
       
    }
        public static async Task<string> HandleUserRequest(AIAgent? agent,IList<AIContent> messageContents, IList<ChatMessage> ChatHistory, AgentSession? session = null )
        {
            await Task.Yield();
            if(session is null)
            {
                session = await agent.CreateSessionAsync();
            }
           
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
