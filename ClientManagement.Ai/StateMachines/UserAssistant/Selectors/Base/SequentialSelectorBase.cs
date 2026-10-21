
using ClientManagement.AI.Constants;
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base
{
    internal class SequentialSelectorBase(UserAssistantContext context) : Interfaces.Base.ISelectorBase<UserAssistantContext, ChatMessage>
    {
        protected UserAssistantContext context = context;
        protected ChatMessage? userMessage;
        virtual public IList<Func<UserAssistantContext, ChatMessage, ChatResponse?, Task<ChatResponse>>> Actions { get; set; } = [];
        virtual public async IAsyncEnumerable<ChatResponse> Receptor(object? sender, ChatMessage payload)
        {
            try
            {
                //await context.WaitAsync();
                userMessage = payload;
                ChatResponse? formerActionResponse = null;
                foreach (var effect in Actions)
                {
                    if (context is { State: { On: true } })
                    { 
                        formerActionResponse =  await effect(context, userMessage, formerActionResponse);
                        
                        if (formerActionResponse?.AdditionalProperties?.TryGetValue(ExecutionStatus.Success, out bool success) ?? false)
                        {
                            if (!success)
                            {
                                break;
                            }
                        }
                        else
                        {
                             break;
                        }
                        yield return formerActionResponse;
                    }
                }
            }
            finally
            {
               // context.Release();
            }
        }
    }
}
