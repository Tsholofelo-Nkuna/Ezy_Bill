using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class UserChatMessageSelect : ISelectorBase<UserAssistantContext>
    {
        private UserAssistantContext? _context;
        private ChatMessage? _userMessage;
        public IEnumerable<Action<UserAssistantContext>> Actions()
        {
            return [
                   GreetUser
                ];
        }

        public void Dispose()
        {
            //Dispose signal
            _context?.IncomingUserMessageSignal?.OnTrigger -= OnSelectorActivated;
        }

        public void SignalListener(UserAssistantContext context)
        {
            _context = context;
            _context.IncomingUserMessageSignal?.OnTrigger += OnSelectorActivated;
        }

        public void OnSelectorActivated(UserIncomingMessageTrigger? intent, ChatMessage? userMessage)
        {
            _userMessage = userMessage;
            foreach (var action in Actions()) {
                try
                {
                    _context?.State?.Wait();
                    if (_context is { State: { On: true } })
                    {
                        action(_context);
                    }
                }
                finally {
                    _context?.State?.Release();
                }
            }
        }

        public void GreetUser(UserAssistantContext context) {
            Console.WriteLine(_userMessage);
        }
    }
}
