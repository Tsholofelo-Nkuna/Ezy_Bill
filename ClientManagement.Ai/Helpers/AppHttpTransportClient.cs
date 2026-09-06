

using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;

using System.ClientModel;

namespace ClientManagement.Ai.Helpers
{
    public class AppHttpTransportClient 
    {

        public IList<McpClientTool> Tools { get; set; }
        public AppHttpTransportClient(IOptions<AgentOptions> agentOptions)
        {
         
            var stdioClientTransport = new HttpClientTransport(new HttpClientTransportOptions()
            {
               Endpoint = new Uri(agentOptions.Value.McpUrl)
            });

            using var client =  McpClient.CreateAsync(stdioClientTransport);

            Tools = client.Result.ListToolsAsync().Result;
            
        }

        
    }
}
