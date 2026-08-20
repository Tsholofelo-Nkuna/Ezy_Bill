using ClientManagement.BusinessLogicLayer.Models;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Helpers
{
    public class AssistantChatApiClient : OllamaApiClient, IChatClient
    {
        public AssistantChatApiClient(IOptions<OllamaOptions> apiOptions): base(apiOptions.Value.Url, apiOptions.Value.Model)
        {
        }
    }
}
