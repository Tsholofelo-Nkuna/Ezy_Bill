using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base
{
    public interface IParentSequentialSelectorBase<TContext, TPayload>: ISelectorBase<TContext, TPayload>
    {
        IList<ISelectorBase<TContext, TPayload>> Children { get; set; }
    }
}
