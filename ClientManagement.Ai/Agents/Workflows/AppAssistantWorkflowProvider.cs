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
            IEnumerable<AIAgent> agents = [bookkeepingAgent.AgentInstance, imageAnalystAgent.AgentInstance, cvAnalystAgent.AgentInstance];
            //return AgentWorkflowBuilder.BuildConcurrent([bookkeepingAgent.AgentInstance, imageAnalystAgent.AgentInstance, cvAnalystAgent.AgentInstance]);
            return AgentWorkflowBuilder.CreateHandoffBuilderWith(imageAnalystAgent.AgentInstance)
                .WithHandoff(imageAnalystAgent.AgentInstance, bookkeepingAgent.AgentInstance, $"User's request is accounting or finance related or access to knowledge source owned by {bookkeepingAgent.AgentInstance.Name} is required.")
                .WithHandoff(imageAnalystAgent.AgentInstance, cvAnalystAgent.AgentInstance, $"User's request is regarding the gathering of information about an individual's identity or professional history or access to knowledge source owned by {cvAnalystAgent.AgentInstance.Name} is required.")
                       //.EnableReturnToPrevious()
                .Build();

            //return AgentWorkflowBuilder.CreateGroupChatBuilderWith(agents => new RoundRobinGroupChatManager(agents) { MaximumIterationCount = agents.Count()}).
            //    AddParticipants(agents)
            //    .Build();
            //    return AgentWorkflowBuilder.CreateMagenticBuilderWith(imageAnalystAgent.AgentInstance)
            //        .AddParticipants(bookkeepingAgent.AgentInstance, cvAnalystAgent.AgentInstance)
            //        .RequirePlanSignoff(false)
            //        .WithMaxStalls(1)
            //        .WithMaxResets(3)
            //        .WithMaxRounds(5)
            //        .Build();
            }


        }
}
