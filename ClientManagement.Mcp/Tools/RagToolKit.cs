

using ClientManagement.DataAccessLayer.Helpers.Interface;
using ClientManagement.Models.AI;
using ClientManagement.Utils.Ai;
using ClientManagement.Utils.Constants.AI;
using Microsoft.Agents.AI;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using System.ComponentModel;



namespace ClientManagement.Mcp.Tools
{
     [McpServerToolType]
    public class  RagToolKit(IVectorStore vectorStore, ILogger<RagToolKit> logger, AgentStoreKeyRegistry agentStoreKeyRegistry, IOptions<AgentOptions> agentOptions)
    {
        [McpServerTool, Description("Adds more context to the user's inquiry, never respond to the user without first using this tool. [Roles: any]")]
        public async Task<string> AddInsightToPrompt(
            [Description("The most recent instruction/question from the user.")] string instruction,
            [Description("Your exact name")] string yourName,
            [Description("Your store key")] string storeKey)
        {
            try
            {
                logger.LogInformation($"{nameof(AddInsightToPrompt)} invoked by {yourName} using {storeKey}");
                _ = agentStoreKeyRegistry.Registry.TryGetValue(storeKey, out var storeKeyOwner);
                var knowledgSourceName = !string.IsNullOrWhiteSpace(storeKeyOwner) ? agentOptions.Value.AiAgentMetaData.FirstOrDefault(aMetaData => aMetaData.Name.Equals(storeKeyOwner, StringComparison.OrdinalIgnoreCase))?.VecStoreMetaData?.Name ?? string.Empty : string.Empty;
                if (string.IsNullOrWhiteSpace(storeKey) || !yourName.Equals(storeKeyOwner, StringComparison.OrdinalIgnoreCase)) //need to assign agents keys that only they know about
                {
                    logger.LogInformation($"{yourName} denied access to {knowledgSourceName}.");
                    return $"{yourName} is not allowed to access this knowledge source ({knowledgSourceName}).";
                }
                var contextItems = (await vectorStore.SearchAsync(knowledgSourceName, instruction, yourName));
                var context = string.Join("\n", contextItems);
                var prompt = $"Respond to the following instruction(s)/question(s) using the provided context.\nQuestion(s)/instruction(s): {instruction}\nContext:\n{context}";
                logger.LogInformation($"{yourName} successfully accessed {knowledgSourceName}");
                return prompt;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, ex.Message);
                return ErrorConstants.UnkownErrorMessage;
            }
        }
    }
}
