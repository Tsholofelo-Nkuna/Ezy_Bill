using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base
{
    public interface ISelectorBase<IContext, TPayload>
    {
        internal IEnumerable<Action<TPayload>> Actions();
        void Receptor(IContext? context, TPayload payload);
        
    }
}
