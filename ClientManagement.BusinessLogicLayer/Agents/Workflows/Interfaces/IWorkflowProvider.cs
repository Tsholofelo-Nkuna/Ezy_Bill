using Microsoft.Agents.AI.Workflows;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents.Workflows.Interfaces
{
    public interface IWorkflowProvider
    {
        Workflow Create();
    }
}
