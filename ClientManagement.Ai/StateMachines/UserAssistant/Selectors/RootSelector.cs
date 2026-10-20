using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class RootSelector(UserAssistantContext context) :  SequentialSelectorBase(context)
    {
        protected UserFileTypeSelector userFileTypeSelector = new(context);
        public async override IAsyncEnumerable<ChatResponse> Receptor(object? sender, ChatMessage payload)
        {
          
            await foreach(var response in userFileTypeSelector.Receptor(sender, payload))
            {
                yield return response;
            }
            await foreach(var response in base.Receptor(sender, payload))
            {
                yield return response;
            }
        }
    }
}
