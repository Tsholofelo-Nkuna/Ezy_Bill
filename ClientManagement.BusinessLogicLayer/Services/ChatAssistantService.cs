using ClientManagement.BusinessLogicLayer.Interfaces;
using Microsoft.Extensions.AI;
using OllamaSharp;
using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ChatAssistantService(IOllamaApiClient client) : IChatAssistantService
    {
        public List<Message> ChatMessages { get; set; } = [];

        public IAsyncEnumerable<ChatResponseStream?> AssistUser(string content)
        {
            ChatMessages.Add(new() { Content = content, Role = new("user")});
            return client.ChatAsync(new() { Messages = ChatMessages });
        }
    }
}
