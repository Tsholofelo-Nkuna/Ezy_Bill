using ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.States
{
    internal class UserAssistantState(int x, int y) : SemaphoreSlim(x,y), IUserAssistantState
    {
        public bool On {  get; set; }
    }
}
