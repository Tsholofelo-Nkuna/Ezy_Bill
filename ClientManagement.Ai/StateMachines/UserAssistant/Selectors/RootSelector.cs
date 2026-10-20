using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class RootSelector : SequentialSelectorBase, IParentSequentialSelectorBase<UserAssistantContext, ChatMessage>
    {
        public RootSelector(UserAssistantContext context) : base(context)
        {
           
            Children.Add(new UserContentValidatorSelector(context));
        }

        public IList<ISelectorBase<UserAssistantContext, ChatMessage>> Children { get; set; } = [];

        public async override IAsyncEnumerable<ChatResponse> Receptor(object? sender, ChatMessage payload)
        {

            foreach (var childSelector in Children) 
            {
                await foreach (var response in childSelector.Receptor(sender, payload))
                {
                    yield return response;
                }
            }
        }
    }
}
