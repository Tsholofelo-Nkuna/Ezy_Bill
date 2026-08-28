using ClientManagement.BusinessLogicLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ClientManagement.BusinessLogicLayer.Agents.Tools
{
    public class  RagToolKit(IAppFileService appFileService)
    {
        [Description("Adds more context about a particular subject mentioned in the user's prompt. This tool returns the original prompt from the user accompanied by insight regarding a particular subject matter mentioned within the user's original request/prompt. use this tool to attach insight to the user's request.")]
        public async Task<string> AddInsightToPrompt([Description("instruction(s)/question(s) from the user")] string userPrompts)
        {
            var contextItems = await appFileService.SearchAsync(userPrompts);
            var context = string.Join("\n", contextItems);
            var prompt = $"respond to the following instruction(s)/question(s) using the provided context.\nQuestion/instruction: {userPrompts}\nContext:\n{context}";
            return prompt;
        }
    }
}
