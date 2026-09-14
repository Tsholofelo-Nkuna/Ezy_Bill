

using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;


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

            using var client = McpClient.CreateAsync(stdioClientTransport);

            Tools = client.Result.ListToolsAsync().Result;

            var transportOptions = new StdioClientTransportOptions
                {
                    Command = "docker",
                    Arguments = new[] { 
                        "run", 
                        "-i", 
                        "--rm", 
                        "-v", "C:/Users/tgnku/OneDrive/Desktop/Desktop:/projects", 
                        "mcp/filesystem", 
                        "/projects" 
                    }
                };

             var transport = new StdioClientTransport(transportOptions);

            using var c = McpClient.CreateAsync(transport);
            Tools = [..Tools, ..c.Result.ListToolsAsync().Result];
        }

        
    }
}
