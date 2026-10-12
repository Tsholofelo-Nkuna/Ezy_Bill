using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base
{
    public interface ITriggerBase<TSender,TPayload>: ITriggerSignal<TPayload>
    {
       public void Fire(TSender sender, TPayload payload);
    }
}
