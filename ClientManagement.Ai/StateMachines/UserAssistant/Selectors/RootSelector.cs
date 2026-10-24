using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class RootSelector : SequentialSelectorBase, IParentSequentialSelectorBase<UserAssistantContext, ChatMessage>
    {
        public RootSelector(UserAssistantContext context) : base(context)
        {
           
            Children.Add(new AgentPersonaSelector(context));
            Children.Add(new DataPartitionSelector(context));
            Children.Add(new DocumentProcessorSelector(context));
          
        }

        public IList<ISelectorBase<UserAssistantContext, ChatMessage>> Children { get; set; } = [];

        public async override IAsyncEnumerable<ChatResponse> Receptor(object? sender, ChatMessage payload)
        {
            context.Logger.LogInformation($"executing {nameof(Receptor)} from {nameof(RootSelector)}");
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
