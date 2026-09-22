

using ClientManagement.Models.AI;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Client;
using System.Text.RegularExpressions;


namespace ClientManagement.Ai.Helpers
{
    public class AppHttpTransportClient 
    {
        private readonly ILogger<AppHttpTransportClient> _logger;

        public IList<McpClientTool> Tools { get; set; }
        public AppHttpTransportClient(IOptions<AgentOptions> agentOptions, ILogger<AppHttpTransportClient> logger)
        {
            this._logger = logger;
            var stdioClientTransport = new HttpClientTransport(new HttpClientTransportOptions()
            {
                Endpoint = new Uri(agentOptions.Value.McpUrl)
            });

            using var client = McpClient.CreateAsync(stdioClientTransport);

            Tools = client.Result.ListToolsAsync().Result;
            
            //var transportOptions = new StdioClientTransportOptions
            //{
            //    Command = "docker",
            //    Arguments = new[] {
            //            "run",
            //            "-i",
            //            "--rm",
            //            "-v", "C:/Users/tgnku/OneDrive/Desktop/Desktop:/projects",
            //            "mcp/filesystem",
            //            "/projects"
            //        }
            //};

            //var transport = new StdioClientTransport(transportOptions);

            //using var c = McpClient.CreateAsync(transport);
            //Tools = [.. Tools, .. c.Result.ListToolsAsync().Result];
           
        }

        /// <summary>
        /// Get a list of tools assocated with the given role
        /// </summary>
        /// <param name="role">name of the role to retrieve tools for</param>
        /// <returns></returns>
        public IEnumerable<McpClientTool> GetTools(string role) => Tools.Select(x =>
        {
            var roleContents = Regex.Match(x.Description, @"\[\s*(role)s?\s*:\s*\w+\]", RegexOptions.IgnoreCase).Value;
            if(roleContents is string validRoleContents)
            {
                var roleList = Regex.Match(validRoleContents,@"(?<=:)(\s*[\w\d]+)").Value.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim().ToLower());
                return roleList.Contains(role.Trim().ToLower()) || roleList.Contains("any") ? x : null;
            }
            else
            {
                return null;
            }
        }).Where(tool => tool is not null) as IEnumerable<McpClientTool>;
    }
}
