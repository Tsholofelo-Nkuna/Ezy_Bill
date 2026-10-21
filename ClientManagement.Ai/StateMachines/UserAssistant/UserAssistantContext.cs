using ClientManagement.AI.StateMachines.UserAssistant.Selectors;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.States;
using ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
namespace ClientManagement.AI.StateMachines.UserAssistant
{
    public class UserAssistantContext(IChatClient chatClient, IOptions<AgentOptions> agentOptions, ILogger<UserAssistantContext> loggerObject) : SemaphoreSlim(1,1)
    {
        internal ChatMessage? UserMessage { get; set; }
        internal string InputForm { get; set; } = string.Empty;
        public EventHandler<string>? HtmlInputFormOutputSignal { get; set; }
        internal  IChatClient ChatClient => chatClient;
        internal  IOptions<AgentOptions> AgentOptions => agentOptions;
        internal UserAssistantState? State { get; set; }
        internal RootSelector? IncomingMessageSelector { get; set; }
        internal ILogger<UserAssistantContext> Logger => loggerObject;
        
        public void SwitchOn()
        {
           
            this.State = new() { On = true};
            IncomingMessageSelector = new(this);
        }

        public void SwitchOff()
        {
            this.State?.On = false;
        }

        public IAsyncEnumerable<ChatResponse>? Receptor(object? sender, ChatMessage? message)
        {
            Logger.LogInformation($"executing {nameof(Receptor)} from {nameof(UserAssistantContext)}");
            if(message is not null)
            {
                return IncomingMessageSelector?.Receptor(sender, message);
            }
            else
            {
                return default;
            }
           
        }

    }
}
