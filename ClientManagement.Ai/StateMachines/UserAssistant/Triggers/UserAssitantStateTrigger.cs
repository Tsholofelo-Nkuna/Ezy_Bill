using ClientManagement.AI.StateMachines.UserAssistant.States;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers
{
    internal abstract class UserAssitantStateTrigger<TSender> : TriggerBase<TSender, UserAssistantState>, ITriggerBase<TSender, UserAssistantState>
    {
    
        public void Fire(TSender sender, UserAssistantState payload)
        {
            Trigger(sender, payload);
        }
    }
}
