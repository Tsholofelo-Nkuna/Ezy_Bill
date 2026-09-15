using ClientManagement.Ai.Agents.Workflows.Interfaces;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Agents.Workflows
{
    public class AppAssistantWorkflowProvider(BookkeepingAgent bookkeepingAgent, ManagerAgent manageAgent, CvAnalystAgent cvAnalystAgent) : IWorkflowProvider
    {
        public Workflow Create()
        {
            IEnumerable<AIAgent> agents = [bookkeepingAgent.AgentInstance, manageAgent.AgentInstance, cvAnalystAgent.AgentInstance];
            //return AgentWorkflowBuilder.BuildConcurrent([bookkeepingAgent.AgentInstance, manageAgent.AgentInstance, cvAnalystAgent.AgentInstance]);
            return AgentWorkflowBuilder.CreateHandoffBuilderWith(manageAgent.AgentInstance)
                .WithHandoff(manageAgent.AgentInstance, bookkeepingAgent.AgentInstance, $"User's request is accounting or finance related or access to knowledge source owned by {bookkeepingAgent.AgentInstance.Name} is required.")
                .WithHandoff(manageAgent.AgentInstance, cvAnalystAgent.AgentInstance, $"User's request is regarding the gathering of information about an individual's identity or professional history or access to knowledge source owned by {cvAnalystAgent.AgentInstance.Name} is required.")
                       //.EnableReturnToPrevious()
                .Build();

            //return AgentWorkflowBuilder.CreateGroupChatBuilderWith(agents => new RoundRobinGroupChatManager(agents) { MaximumIterationCount = agents.Count()}).
            //    AddParticipants(agents)
            //    .Build();
            //    return AgentWorkflowBuilder.CreateMagenticBuilderWith(manageAgent.AgentInstance)
            //        .AddParticipants(bookkeepingAgent.AgentInstance, cvAnalystAgent.AgentInstance)
            //        .RequirePlanSignoff(false)
            //        .WithMaxStalls(1)
            //        .WithMaxResets(3)
            //        .WithMaxRounds(5)
            //        .Build();
            }


        }
}
