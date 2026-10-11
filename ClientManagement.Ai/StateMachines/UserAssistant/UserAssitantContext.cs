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
    }
}
