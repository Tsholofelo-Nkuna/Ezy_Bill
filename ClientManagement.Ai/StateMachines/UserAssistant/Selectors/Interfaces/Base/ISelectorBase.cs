using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base
{
    internal interface ISelectorBase<IContext>: IDisposable
    {
        void SignalListener(IContext context);
        IEnumerable<Action<IContext>> Actions();
        
    }
}
