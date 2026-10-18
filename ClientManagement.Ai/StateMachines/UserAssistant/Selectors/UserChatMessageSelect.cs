using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.DataAccessLayer.Helpers.Interface;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;
using OllamaSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    public class UserChatMessageSelect(UserAssistantContext context) : SequentialSelectorBase(context), ISelectorBase<UserAssistantContext, ChatMessage>
    {
        
        override public IEnumerable<Func<UserAssistantContext, ChatMessage, Task<ChatResponse>>> Actions()
        {
            return [
                   RagTool,
                ];
        }

        internal  Task<ChatResponse> RagTool(UserAssistantContext context ,ChatMessage userMessage)
        {
            //var 
            //vectorStore.SearchAsync()
            return Task.FromResult(new ChatResponse([new(ChatRole.Assistant, [])]));
        }

        
       

    }
}
