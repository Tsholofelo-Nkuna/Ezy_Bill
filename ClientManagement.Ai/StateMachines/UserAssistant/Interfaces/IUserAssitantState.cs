using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.Interfaces
{
    internal interface IUserAssitantState
    {
        public void PresentInputForm(UserAssitantContext context);
    }
}
