using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base
{
    public class TriggerBase<TSender, TPayload> : ITriggerBase<TSender, TPayload>
    {
        [Obsolete]
        public TPayload? Payload { get ; set ; }
        [Obsolete]
        protected EventHandler<TPayload?>? triggerSignal {  get; set; }
        public event ITriggerBase<TSender, TPayload>.TriggerHandler? OnTrigger;
        public void AddListener(ITriggerBase<TSender, TPayload>.TriggerHandler handler)
        {
            OnTrigger += handler;
        }

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

        [Obsolete("Use OnTrigger")]
        public EventHandler<TPayload?>? GetSignal()
        {
            return triggerSignal;
        }

        virtual public void Listen(TSender sender, TPayload payload)
        {
            throw new NotImplementedException();
        }
    }
}
