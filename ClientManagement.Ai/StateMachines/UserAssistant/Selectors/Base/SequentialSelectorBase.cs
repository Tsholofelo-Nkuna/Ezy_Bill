
using DocumentFormat.OpenXml.InkML;
using Microsoft.Extensions.AI;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base
{
    public class SequentialSelectorBase(UserAssistantContext context) : Interfaces.Base.ISelectorBase<UserAssistantContext, ChatMessage>
    {
        protected UserAssistantContext _context = context;
        protected ChatMessage? _userMessage;
        virtual public IEnumerable<Func<UserAssistantContext, ChatMessage, Task<ChatResponse>>> Actions()
        {
            return [
                   
                ];
        }
        virtual public async IAsyncEnumerable<ChatResponse> Receptor(UserAssistantContext? context, ChatMessage payload)
        {
            try
            {
                await _context.WaitAsync();
                _userMessage = payload;
                foreach (var effect in Actions())
                {
                    if (_context is { State: { On: true } })
                    {
                        yield return await effect(_context, _userMessage);
                    }
                }
            }
            finally
            {
                _context.Release();
            }
        }
    }
}
