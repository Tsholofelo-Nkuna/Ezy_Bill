

using ClientManagement.DataAccessLayer.Helpers.Interface;
using ModelContextProtocol.Server;
using System.ComponentModel;



namespace ClientManagement.Mcp.Tools
{
     [McpServerToolType]
    public class  RagToolKit(IVectorStore vectorStore)
    {
        [McpServerTool, Description("Adds more context to the user's inquiry, never respond to the user without first using this tool.")]
        public async Task<string> AddInsightToPrompt(
            [Description("The exact name of the owner of the knowledge source.")] string agentName,
            [Description("The exact name of the knowledge source.")] string knowledgSourceName,
            [Description("The most recent instruction/question from the user.")] string instruction,
            [Description("Your exact name")] string yourName)
        {
            if (!yourName.Equals(agentName, StringComparison.OrdinalIgnoreCase))
            {
                return $"{yourName} is not allowed to access this knowledge source ({knowledgSourceName}) it's owned by {agentName}.";
            }
            var contextItems = (await vectorStore.SearchAsync(knowledgSourceName, instruction, agentName));
            var context = string.Join("\n", contextItems);
            var prompt = $"Respond to the following instruction(s)/question(s) using the provided context.\nQuestion(s)/instruction(s): {instruction}\nContext:\n{context}";
            //var handOfInstruction = !yourName.Equals(agentName, StringComparison.OrdinalIgnoreCase) ? "You should handoff this inquiry to the appropriate specialist" : string.Empty;
            //prompt += $"\n{handOfInstruction}";
            return prompt;
        }
    }
}
