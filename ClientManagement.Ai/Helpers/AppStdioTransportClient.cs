

using ClientManagement.Ai.Models;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;

using System.ClientModel;

namespace ClientManagement.Ai.Helpers
{
    public class AppStdIoTransportClient 
    {

        public IList<McpClientTool> Tools { get; set; }
        public AppStdIoTransportClient(IOptions<AgentOptions> agentOptions)
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
