using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base
{
    [Obsolete]
    public interface ITriggerSignal<TSignalPayload>
    {
        [Obsolete]
        EventHandler<TSignalPayload?>? GetSignal();
        [Obsolete]
        TSignalPayload? Payload { get; set; }
    }
}
