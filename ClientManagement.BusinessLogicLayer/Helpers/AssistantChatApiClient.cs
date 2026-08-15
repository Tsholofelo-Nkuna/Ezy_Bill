using Microsoft.Extensions.AI;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Helpers
{
    public class AssistantChatApiClient : OllamaApiClient, IChatClient
    {
        public AssistantChatApiClient(): base("http://localhost:11434", "llama3.2:3b")
        {
            var models = this.ListLocalModelsAsync().Result;
        }
    }
}
