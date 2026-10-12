using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Triggers.Interfaces.Base
{
    public interface ITriggerSignal<TSignalPayload>
    {
        EventHandler<TSignalPayload?>? GetSignal();
    }
}
