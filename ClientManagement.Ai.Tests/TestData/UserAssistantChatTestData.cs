using ClientManagement.Models.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.Ai.Tests.TestData
{
    internal class UserAssistantChatTestData : TheoryData<string, AgentOptions, string>
    {
        internal UserAssistantChatTestData_UserAssitant_Works()
        {
            var aOptions = new AgentOptions()
            {
                 AiAgentMetaData = [
                     new(){
                        Profession = "Doctor"
                     }
                 ]
            }

            this.Add("I am sick", aOptions, "yes")
            this.Add("I need money", aOptions, "no")
            this.Add("exit", aOptions, "no")
        }
    }
}
