using ClientManagement.BusinessLogicLayer.Agents.Tools;
using ClientManagement.BusinessLogicLayer.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents
{
    public class ImageAnalystAgent : AgentBase
    {
        public ImageAnalystAgent(IOptions<AgentOptions> agentOptions, RagToolKit ragToolKit) : base(agentOptions, ragToolKit)
        {
            Name = "Paul";
        }
    }
}
