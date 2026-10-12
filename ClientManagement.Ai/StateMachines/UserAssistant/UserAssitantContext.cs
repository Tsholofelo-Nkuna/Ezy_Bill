using ClientManagement.AI.StateMachines.UserAssistant.Interfaces.Base;
using ClientManagement.AI.StateMachines.UserAssistant.States;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant
{
    public class UserAssitantContext(IChatClient chatClient, IOptions<AgentOptions> agentOptions)
    {
        internal ChatMessage? UserMessage { get; set; }
        internal string InputForm { get; set; } = string.Empty;
        public EventHandler<string>? HtmlInputFormOutputSignal { get; set; }
        internal  IChatClient ChatClient => chatClient;
        internal  IOptions<AgentOptions> AgentOptions => agentOptions;
        internal IEnumerable<Triggers.Interfaces.Base.ITriggerSignal<object?>> EnvironmentSignals { get; set; } = [];
        internal UserAssitantState? State { get; set; }
        public UserIncomingMessageTrigger? IncomingUserMessageSignal { get; set; }
        public Task Initialize()
        {
            IncomingUserMessageSignal = new UserIncomingMessageTrigger(this);
            EnvironmentSignals = [
                (IncomingUserMessageSignal as ITriggerSignal<object?>)!
            ];
            foreach(var signalWrapper in EnvironmentSignals)
            {
                var signal = signalWrapper.GetSignal();
                signal += OnTrigger;
            }
            State = new UserAssitantState();
            return Task.CompletedTask;
        }

        internal void OnTrigger(object? sender, object? payload)
        {
            if(sender is UserIncomingMessageTrigger userIncommingMessageTrigger)
            {
                State?.PresentInputForm(this);
            }
        }
    }
}
