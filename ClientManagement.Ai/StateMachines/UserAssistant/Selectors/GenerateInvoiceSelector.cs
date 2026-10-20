using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class GenerateInvoiceSelector(UserAssistantContext context) : SequentialSelectorBase(context), ISelectorBase<UserAssistantContext, ChatMessage>
    {
        
     

        internal  Task<ChatResponse> RagTool(UserAssistantContext context ,ChatMessage userMessage, ChatResponse? formerActionResponse)
        {
            //var 
            //vectorStore.SearchAsync()
            
            return Task.FromResult(new ChatResponse([new(ChatRole.Assistant, [])]));
        }

    }
}
