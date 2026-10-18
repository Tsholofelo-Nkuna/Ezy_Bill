using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base
{
    public interface ISelectorBase<IContext, TPayload>
    {
      
        internal IEnumerable<Func<UserAssistantContext, TPayload, Task<ChatResponse>>> Actions();
        IAsyncEnumerable<ChatResponse> Receptor(IContext? context, TPayload payload);
      


    }
}
