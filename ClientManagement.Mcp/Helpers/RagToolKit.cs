using ClientManagement.Models.AI;
using Microsoft.Extensions.Options;

using System.ComponentModel;

using Core.Utils.Interfaces;
using OllamaSharp;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ModelContextProtocol.Server;
using ClientManagement.BusinessLogicLayer.Helpers.Interface;

namespace ClientManagement.Mcp.Helpers
{
    [McpServerToolType]
    public class  RagToolKit(IVectorStore vectorStore)
    {
        [McpServerTool, Description("Adds more context to the user's inquiry, never respond to the user without first using this tool.")]
        public async Task<string> AddInsightToPrompt(
            [Description("The name (your name) of the agent currently attending the user's inquiry")] string agentName,
            [Description("The name of the knowledge source which is most likely to contain additional context about the user's inquiry.")] string releventKnowledgSourceName,
            [Description("The most recent instruction/question from the user.")] string instruction,
            [Description("The name of the owner of the knowledge source which is most likely to contain context about the user's inquiry")] string relevantSourceOwner)
        {
            
            var contextItems = (await vectorStore.SearchAsync(releventKnowledgSourceName, instruction, relevantSourceOwner));
            var context = string.Join("\n", contextItems);
            var prompt = $"Respond to the following instruction(s)/question(s) using the provided context.\nQuestion(s)/instruction(s): {instruction}\nContext:\n{context}";
            return prompt;
        }
    }
}
