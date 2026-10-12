using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces
{
    public interface IUserAssitantState
    {
        public void PresentInputForm(UserAssitantContext context);
    }
}
