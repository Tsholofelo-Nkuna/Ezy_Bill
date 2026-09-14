using ClientManagement.Ai.Agents.Enums;
using ClientManagement.Ai.Agents.Interfaces;
using ClientManagement.Ai.Helpers;
using ClientManagement.AI.Constants;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace ClientManagement.Ai.Agents
{
    public abstract class AgentBase : AssistantChatApiClient, IAgentBase
    {
        public List<ChatMessage> ChatHistory = [];
        protected readonly IOptions<AgentOptions> agentOptions;
        protected readonly AssistantChatApiClient chatClient;
        private readonly AppHttpTransportClient stdIoTransportClient;
        private readonly IVectorStore _vectorStore;

        public event EventHandler<string> ChatResponseReceived;
        public AgentBase(IOptions<AgentOptions> agentOptions,  AssistantChatApiClient chatClient, AppHttpTransportClient stdIoTransportClient, IVectorStore vectorStore) : base(agentOptions)
        {
            this.agentOptions = agentOptions;
            this.chatClient = chatClient;
            this.stdIoTransportClient = stdIoTransportClient;
            this._vectorStore = vectorStore;

            var baseSkillPath = this.agentOptions.Value.SkillPath;
            var replacement = "Ai";
            DirectoryInfo skillsDirInfo = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory.Replace("Presentation.Web",replacement), baseSkillPath));
            var agentDirNames = this.agentOptions.Value.AiAgentMetaData.Select(x => new DirectoryInfo(Path.Combine(AppContext.BaseDirectory.Replace("Presentation.Web", replacement), x.SkillPath)));
            skillsDirInfo
                .GetDirectories().Where(x => !agentDirNames.Select(x => x.FullName).Contains(x.FullName))
                .ToList()
                .ForEach(sourceDir => {
                    sourceDir.EnumerateFiles().ToList().ForEach(f =>
                    {
                        var newFilePath = Path.Combine(f.Directory.Name, f.Name);
                        var names = agentDirNames.Select(aD => Path.Combine(aD.FullName, newFilePath));
                        foreach (var item in names)
                        {
                            File.Copy(f.FullName, item, true);
                        }
                       
                    });
                    //return dirContents;
                });
        } 
       // public AgentSkillsProvider BaseSkill => new AgentSkillsProvider(Path.Combine(AppContext.BaseDirectory, agentOptions.Value.SkillPath));
        public AgentSkillsProvider Skill => new AgentSkillsProvider(Path.Combine(AppContext.BaseDirectory, agentOptions.Value.AiAgentMetaData.FirstOrDefault(x => x.Name == Name)?.SkillPath));

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
            var knowledgeBaseContextStringForMaster = $"The following is a list of available knowledge source names along with their descriptions and owners (you are amongst the owners, the source you have access to is listed with your name): {string.Join("", knoweledgeBaseContext)}. **Access to knowledge source rule**: Only the owner of the knowledge source has access to it; you should always delegate any request for information from their knowledge sources to them.";
          
            var knowledgeSourceName = agentMetaData.FirstOrDefault()?.VecStoreMetaData?.Name;
            var knowledgeBaseContextStringForWorkers = $"You are the sole owner of a knowledge source called {knowledgeSourceName}, treat it as your source of truth as it's the only source of information you have.";
            //var knowledgeSourceString = !string.IsNullOrWhiteSpace(knowledgeSourceName) ? $"You have access to a single knowledge source called {knowledgeSourceName}" : "Always handoff the user's inquiry if it's outside your area of expertise";
            var agent = agentMetaData
                 .Select(aMetaData =>
                 {
                     var kBStr = aMetaData.Type.Equals(AgentType.Master, StringComparison.OrdinalIgnoreCase) ?  knowledgeBaseContextStringForMaster : knowledgeBaseContextStringForWorkers;
                    
                     var options = new ChatClientAgentOptions()

                     {
                         AIContextProviders = [Skill],
                         ChatOptions = new()
                         {
                             Instructions = $"Your name is {agentName}. {kBStr}. {aMetaData.Instructions}. All your responses should be in plain text. Never mention your internal tools.",
                             Tools = [.. this.stdIoTransportClient.Tools],
                             ModelId = string.IsNullOrWhiteSpace(aMetaData.Model) ? this.agentOptions.Value.OllamaModel : aMetaData.Model,
                             AdditionalProperties =new AdditionalPropertiesDictionary { [AgentRunOptionProperties.AgentName] = agentName }

                         },
                         Name = aMetaData.Name,
                         Description = aMetaData.Description,
                     };
                     var keys = options.AIContextProviders.Select(x => x.StateKeys);
                     return chatClient.AsAIAgent(options: options);//.AsBuilder()
                     //.Use(sharedFunc: InspectInputMiddleware).Build();// new ChatClientAgent(chatClient, options);
                 }).FirstOrDefault();
            
            return agent;
        }

        //public async Task<string> HandleUserRequest(AIAgent? agent,IList<AIContent> messageContents, IList<ChatMessage> ChatHistory, AgentSession? session = null )
        //{
        //   this.H
        //}

        public ValueTask<AgentSession> CreateSessionAsync()
        {
            return AgentInstance!.CreateSessionAsync(); 
        }
    }
}
