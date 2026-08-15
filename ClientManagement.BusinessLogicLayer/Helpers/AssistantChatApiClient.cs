using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Helpers
{
    public class AssistantChatApiClient : OllamaApiClient, IOllamaApiClient
    {
        public AssistantChatApiClient(): base("http://localhost:11434", "llama3.2:3b")
        {
            var models = this.ListLocalModelsAsync().Result;
        }
    }
}
