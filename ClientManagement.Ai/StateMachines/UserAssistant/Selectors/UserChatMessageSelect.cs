using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    public class UserChatMessageSelect(UserAssistantContext context) : ISelectorBase<UserAssistantContext, ChatMessage>
    {
        private UserAssistantContext _context = context;
        private ChatMessage? _userMessage;
        public IEnumerable<Action<ChatMessage>> Actions()
        {
            return [
                   GreetUser
                ];
        }
        internal void GreetUser(ChatMessage context) {
            Console.WriteLine(_userMessage);
        }

        public void Receptor(UserAssistantContext? context, ChatMessage payload)
        {
            _userMessage = payload;
            foreach (var action in Actions())
            {
                if (_context is { State: { On: true } })
                {
                    action(_userMessage);
                }
            }
        }
    }
}
