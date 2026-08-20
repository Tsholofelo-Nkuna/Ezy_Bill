using Microsoft.Agents.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents.Interfaces
{
    public interface IAgentBase
    {
        ChatClientAgent? AgentInstance { get;  set; }
        string Name { get; set; }
        ChatClientAgent Instance(string agentName);

    }
}
