using ClientManagement.AI.StateMachines.UserAssistant.Selectors;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.States;
using ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
namespace ClientManagement.AI.StateMachines.UserAssistant
{
    public class UserAssistantContext(IChatClient chatClient, IOptions<AgentOptions> agentOptions)
    {
        internal ChatMessage? UserMessage { get; set; }
        internal string InputForm { get; set; } = string.Empty;
        public EventHandler<string>? HtmlInputFormOutputSignal { get; set; }
        internal  IChatClient ChatClient => chatClient;
        internal  IOptions<AgentOptions> AgentOptions => agentOptions;
        internal UserAssistantState? State { get; set; }
        public UserChatMessageSelect? IncomingMessageSelector { get; set; }
        public void SwitchOn()
        {
           
            this.State = new(1, 1) { On = true};
            IncomingMessageSelector = new(this);
        }

        public void SwithOff()
        {
            this.State?.On = false;
        }

    }
}
