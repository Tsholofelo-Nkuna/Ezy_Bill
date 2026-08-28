using ClientManagement.BusinessLogicLayer.Agents.Workflows.Interfaces;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents.Workflows
{
    public class AppAssistantWorkflowProvider(BookkeepingAgent bookkeepingAgent, ImageAnalystAgent imageAnalystAgent) : IWorkflowProvider
    {
        public Workflow Create()
        {
            return AgentWorkflowBuilder.CreateHandoffBuilderWith(imageAnalystAgent.AgentInstance)
                .WithHandoff(imageAnalystAgent.AgentInstance, bookkeepingAgent.AgentInstance)
                .Build();
        }

 
    }
}
