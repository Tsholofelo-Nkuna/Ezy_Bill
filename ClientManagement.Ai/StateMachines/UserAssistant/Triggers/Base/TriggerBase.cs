using ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Base
{
    public class TriggerBase<TSender, TPayload> : ITriggerBase<TSender, TPayload>
    {
        private EventHandler<TPayload?>? _triggerSignal {  get; set; }

        public void Fire(TSender sender, TPayload payload)
        {
            GetSignal()?.Invoke(this, payload);
        }

        public EventHandler<TPayload?>? GetSignal()
        {
            return _triggerSignal;
        }
    }
}
