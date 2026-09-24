using ClientManagement.Models.AI;
using Microsoft.Extensions.Logging;
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

        public AgentStoreKeyRegistry(IOptions<AgentOptions> agentOptions, ILogger<AgentStoreKeyRegistry> logger) 
        {
            logger.LogInformation("Loading agent store key registry");
            var fileContents = File.ReadAllText(agentOptions.Value.AgentStorageKeyFilePath);
            Registry = JsonSerializer.Deserialize<Dictionary<string, string>>(fileContents) ?? new Dictionary<string, string>();
            logger.LogInformation($"Agent store key registry loaded. Found {Registry.Values.Count()} entries");
        }
    }
}
