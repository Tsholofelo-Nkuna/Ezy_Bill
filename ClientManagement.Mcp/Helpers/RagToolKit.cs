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
        [McpServerTool, Description("Adds more context to the user's inquiry, never respond to the user without first using this tool. You should also make use of this tool right after each handoff")]
        public async Task<string> AddInsightToPrompt([Description("The name of the knowledge source")] string knowledgeSourceName, [Description("The most recent instruction/question from the user.")] string instruction, [Description("The name of the agent currently handling the user's request")] string agentName)
        {
            //var appropriateAgentOptionsQuery = "Below is a list of individuals along side descriptions of their duties; These individuals are your colleagues, you can always ask for permission from the user whether they would like their request to be forwarded to them (your colleagues) incase you are unable to resolve their query. Provide the name of the individual whom you think is best suited to handle the user's request.\n\n";
            //appropriateAgentOptionsQuery += "**Individuals along side their professions:**\n";
            //foreach (var agentOp in agentOptions.Value.AiAgentMetaData)
            //{
            //    if(agentName.Contains(agentOp.Name, StringComparison.OrdinalIgnoreCase))
            //    {
            //        continue;
            //    }
            //    appropriateAgentOptionsQuery += $"{agentOp.Name}: {agentOp.Instructions}\n";
            //}

            //appropriateAgentOptionsQuery += "\n\n";
            //appropriateAgentOptionsQuery += $"**User's request:**\n{instruction}";
            //var appropriateAgentReponse = assistantChatApi.GenerateAsync(request: new()
            //{
            //    Prompt = appropriateAgentOptionsQuery,
            //});
            //AgentMetaData? suggestedAgentMetaData;
            //var suggestedAgent = string.Empty ;
            //await foreach(var response in appropriateAgentReponse)
            //{
            //    suggestedAgent += response?.Response;//?.Content?.Aggregate("", (carry, next) => carry + next.ToString()) ?? string.Empty;
            //}
            //suggestedAgentMetaData = agentOptions.Value.AiAgentMetaData.FirstOrDefault(x => suggestedAgent.Contains(x.Name, StringComparison.OrdinalIgnoreCase));
            //var agentMetaData = agentOptions.Value.AiAgentMetaData.FirstOrDefault(x => agentName.Contains(x.Name, StringComparison.OrdinalIgnoreCase));
            //var disclaimer = string.Empty;
            //if(suggestedAgentMetaData is { Name: string } suggestedMetaData && agentMetaData is { Name : string} metaData && !suggestedMetaData.Name.Equals(metaData.Name))
            //{
            //    disclaimer = $"As a disclaimer, mention to the user that you ({agentName}) have consulted with {suggestedMetaData.Name} due to you not having sufficient context to process their request";
            //}

            //if (!string.IsNullOrWhiteSpace(disclaimer))
            //{
            //    agentMetaData = suggestedAgentMetaData;
            //}

            var contextItems = (await vectorStore.SearchAsync(knowledgeSourceName, instruction, agentName));
            var context = string.Join("\n", contextItems);
            var prompt = $"Respond to the following instruction(s)/question(s) using the provided context.\nQuestion(s)/instruction(s): {instruction}\nContext:\n{context}";
            return prompt;
        }
    }
}
