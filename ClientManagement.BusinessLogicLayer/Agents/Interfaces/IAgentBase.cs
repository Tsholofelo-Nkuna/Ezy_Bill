using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents.Interfaces
{
    public interface IAgentBase
    {
        ChatClientAgent? AgentInstance { get; }
        string Name { get; set; }
        ChatClientAgent? Instance(string agentName);
        ValueTask<AgentSession> CreateSessionAsync();

    }
}
