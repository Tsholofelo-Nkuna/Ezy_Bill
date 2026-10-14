using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base
{
    public interface ITriggerBase<TSender,TPayload>: IDisposable
    {
       delegate void TriggerHandler(TSender sender, TPayload payload);
       void Trigger(TSender sender, TPayload payload);
       event TriggerHandler? OnTrigger;

    }
}
