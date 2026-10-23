using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    [Description("Identifies the agent's goal, and decides which tools to use along with the order of execution of those tools.")]
    internal class AgentToolSelector: SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public AgentToolSelector(UserAssistantContext context) : base(context)
        {
        }
    }
}
