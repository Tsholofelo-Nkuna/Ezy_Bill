using ClientManagement.Ai.Agents.Workflows.Interfaces;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents.Workflows
{
    public class AppAssistantWorkflowProvider(BookkeepingAgent bookkeepingAgent, ImageAnalystAgent imageAnalystAgent, CvAnalystAgent cvAnalystAgent) : IWorkflowProvider
    {
        public Workflow Create()
        {
            return AgentWorkflowBuilder.CreateHandoffBuilderWith(imageAnalystAgent.AgentInstance)
                .WithHandoff(imageAnalystAgent.AgentInstance, bookkeepingAgent.AgentInstance, handoffReason: "User's request is accounting or finance related.")
                .WithHandoff(imageAnalystAgent.AgentInstance, cvAnalystAgent.AgentInstance, handoffReason: "User's request is regarding the gathering of information about an individual's identity or professional history.")
                .WithHandoff(bookkeepingAgent.AgentInstance, imageAnalystAgent.AgentInstance, handoffReason:"User's request is related to interpeting or describing visual content")
                 .WithHandoff(cvAnalystAgent.AgentInstance, imageAnalystAgent.AgentInstance, handoffReason: "User's request is related to interpeting or describing visual content")
                .EnableReturnToPrevious()
                .Build();
        }

 
    }
}
