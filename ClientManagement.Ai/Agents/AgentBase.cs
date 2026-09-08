using ClientManagement.Ai.Agents.Interfaces;
using ClientManagement.Ai.Helpers;
using ClientManagement.Models.AI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;

namespace ClientManagement.Ai.Agents
{
    public abstract class AgentBase : AssistantChatApiClient, IAgentBase
    {
        public List<ChatMessage> ChatHistory = [];
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly AssistantChatApiClient chatClient;
        private readonly AppHttpTransportClient stdIoTransportClient;

        public AgentBase(IOptions<AgentOptions> agentOptions,  AssistantChatApiClient chatClient, AppHttpTransportClient stdIoTransportClient) : base(agentOptions)
        {
            this.agentOptions = agentOptions;
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
            var agentMetaData = this.agentOptions.Value.AiAgentMetaData.Where(x => x.Name.Equals(agentName, StringComparison.OrdinalIgnoreCase));
            var knoweledgeBaseContext = this.agentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData : { Name: string, Description: string } }).Select(x =>
            {
                return $"- Name: {x.VecStoreMetaData!.Name}, Description: {x.VecStoreMetaData!.Description}, Owner: {x.Name}";
            });
            var knowledgeBaseContextString = $"The following is a list of available knowledge source names along with their descriptions and owners: {string.Join("", knoweledgeBaseContext)}";
            var knowledgeSourceName = agentMetaData.FirstOrDefault()?.VecStoreMetaData?.Name;
            var agent = agentMetaData
                 .Select(aMetaData =>
                 {
                     var options = new ChatClientAgentOptions()
                     {
                         AIContextProviders = [BaseSkill],
                         ChatOptions = new()
                         {
                             Instructions = $"Your name is {agentName}. {knowledgeBaseContextString}.\nYou have access to a single knowledge source called {knowledgeSourceName ?? "undefined"}. {aMetaData.Instructions}. All your responses should be in plain text. Never mention your internal tools. Always be kind, helpful and use a professional tone.",
                             Tools = [..this.stdIoTransportClient.Tools],
                             ModelId = string.IsNullOrWhiteSpace(aMetaData.Model) ? this.agentOptions.Value.OllamaModel : aMetaData.Model,
                             
                         },
                         Name = aMetaData.Name,
                         Description = aMetaData.Description,
                         
                         
                     };
                     return chatClient.AsAIAgent(options: options);//.AsBuilder().Use(sharedFunc: InspectInputMiddleware).Build();// new ChatClientAgent(chatClient, options);
                 }).FirstOrDefault();
            
            return agent;
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
