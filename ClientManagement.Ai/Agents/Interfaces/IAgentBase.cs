using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents.Interfaces
{
    public interface IAgentBase 
    {
        AIAgent? AgentInstance { get; }
        string Name { get; set; }
        AIAgent? Instance(string agentName);
        ValueTask<AgentSession> CreateSessionAsync();

    }
}
