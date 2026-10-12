using ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.States
{
    internal class UserAssitantState : IUserAssitantState
    {
        public void PresentInputForm(UserAssitantContext context)
        {
           context.HtmlInputFormOutputSignal?.Invoke(context,$"<bold>{context.UserMessage}</bold>");
        }
    }
}
