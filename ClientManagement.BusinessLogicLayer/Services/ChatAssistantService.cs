
//using ClientManagement.Ai.Helpers;
//using ClientManagement.BusinessLogicLayer.Interfaces;
//using Microsoft.Agents.AI;
//using Microsoft.Extensions.AI;
//using OllamaSharp;
//using OllamaSharp.Models.Chat;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace ClientManagement.BusinessLogicLayer.Services
//{
//    [Obsolete]
//    public class ChatAssistantService(AssistantChatApiClient client) : IChatAssistantService
//    {
//        public List<Message> ChatMessages { get; set; } = [];

//        public ChatClientAgent AsChatAgent(string? instructions = null, string? agentName = null, IList<AITool>? tools = null, IEnumerable<AIContextProvider>? contextProviders = null)
//        {
//            return client.AsAIAgent(new ChatClientAgentOptions() { 
//                Name = agentName, 
//                ChatOptions = new() { Tools = tools, Instructions = instructions} }
//            );
            
//        }

//        public IAsyncEnumerable<ChatResponseStream?> AssistUser(string content)
//        {
//            ChatMessages.Add(new() { Content = content, Role = new("user")});
//            return client.ChatAsync(new() { Messages = ChatMessages });
//        }
//    }
//}
