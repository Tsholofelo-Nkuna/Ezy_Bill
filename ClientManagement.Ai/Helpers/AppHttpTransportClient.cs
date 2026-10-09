

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
            var commaDelimitedListOfRolesPatternMatch = Regex.Match(x.Description, @"\[\s*(role)s?\s*:\s*(\w+\s*,?\s*)+\]", RegexOptions.IgnoreCase);
            if (!commaDelimitedListOfRolesPatternMatch.Success)
            {
                _logger.LogError($"The role list in the description of the tool({x.Name}) fails the regular expression pattern match. Rule of thumb, the role list embedded in the descriptoin of the tool must be of a particular format, it must be a comma separeted list of words that strictly make use of `upper case pascal` syntax. i.e [Roles: Admin, Bookkeeper, SystemAnalyst]");
            }
            var roleContents = commaDelimitedListOfRolesPatternMatch.Value;
            _logger.LogInformation($"Tool({x.Name}) is associated with these ({roleContents}) roles");
            if(roleContents is string validRoleContents)
            {
                var roleList = Regex.Match(validRoleContents, @"(?<=:)\s*(\w+\s*,?\s*)+").Value.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(y => y.Trim().ToLower());
                _logger.LogInformation($"Checking if {x.Name} can be accessed by role ({role})");
                var roleCanAccessTool =  roleList.Contains(role.Trim().ToLower()) || roleList.Contains("any") ? x : null;
                _logger.LogInformation($"Role (role) can access tool ({x.Name}): {roleCanAccessTool?.ToString() == bool.TrueString}");
                return roleCanAccessTool;
            }
            else
            {
                return null;
            }
        }).OfType<McpClientTool>();
    }
}
