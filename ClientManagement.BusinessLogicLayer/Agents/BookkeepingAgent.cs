using ClientManagement.BusinessLogicLayer.Agents.Interfaces;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Tsp;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents
{
    public class BookkeepingAgent : AgentBase
    {
        public BookkeepingAgent(IOptions<AgentOptions> agentOptions, IOptions<OllamaOptions> ollamaOptions) : base(agentOptions, ollamaOptions)
        {
        }
    }
}
