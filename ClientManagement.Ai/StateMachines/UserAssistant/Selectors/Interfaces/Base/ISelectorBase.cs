using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base
{
    public interface ISelectorBase<TContext, TPayload>
    {
      
        internal IList<Func<TContext, TPayload, ChatResponse?, Task<ChatResponse>>> Actions { get; set; }
        IAsyncEnumerable<ChatResponse> Receptor(object? sender, TPayload payload);
      


    }
}
