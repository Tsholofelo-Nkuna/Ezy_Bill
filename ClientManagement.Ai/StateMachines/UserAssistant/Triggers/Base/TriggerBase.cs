using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base
{
    public class TriggerBase<TSender, TPayload> :  ITriggerBase<TSender, TPayload>
    {
       
        public event ITriggerBase<TSender, TPayload>.TriggerHandler? OnTrigger;
     
        public void Dispose()
        {
            OnTrigger= null;
        }

        public void Trigger(TSender sender, TPayload payload)
        {
            if(OnTrigger is not null)
            {
                OnTrigger(sender, payload);
            }   
        }
    }
}
