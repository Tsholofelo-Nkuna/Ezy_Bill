using ClientManagement.AI.StateMachines.UserAssistant.States.Interfaces;
using ClientManagement.Models.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant.States
{
    internal class UserAssistantState: IUserAssistantState
    {
        public bool On {  get; set; }
        public AgentMetaData? Persona {  get; set; }
        public int DraftSize { get; set; } = 80;
    }
}
