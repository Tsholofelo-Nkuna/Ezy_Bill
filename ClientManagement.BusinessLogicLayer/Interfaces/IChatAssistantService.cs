using OllamaSharp.Models.Chat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IChatAssistantService
    {
        public IAsyncEnumerable<ChatResponseStream?> AssistUser(string content);
        public List<Message> ChatMessages { get; set; }
    }
}
