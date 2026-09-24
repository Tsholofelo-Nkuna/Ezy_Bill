using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace ClientManagement.Utils.Ai
{
    public class AgentStoreKeyRegistry
    {
        public IDictionary<string, string> Registry { get; init; }

        public AgentStoreKeyRegistry(IOptions<AgentOptions> agentOptions) 
        {
            Registry = JsonSerializer.Deserialize<Dictionary<string, string>>(agentOptions.Value.AgentStorageKeyFilePath) ?? new Dictionary<string, string>();
        }
    }
}
