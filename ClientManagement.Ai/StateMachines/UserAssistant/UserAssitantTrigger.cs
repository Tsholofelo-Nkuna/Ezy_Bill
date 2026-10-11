using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI.StateMachines.UserAssistant
{
   public static class UserAssitantTrigger
    {
        extension(UserAssitantContext context)
        {
            public void UserTextInputSignal(object? sender, ChatMessage userMessage)
            {
                context.UserMessage = userMessage;
            }

            internal async Task<bool> ValidateUserMessage(ChatMessage userMessage) {
                var knowledgeSourceDescriptions = context.AgentOptions.Value.AiAgentMetaData
                     .Select(x => x.Profession)
                     .OfType<string>();
                var professions = string.Join(", ", knowledgeSourceDescriptions);
                var llmPrompt = $"""
                    Does the inquiry align with any of the specified professions? Respond with a yes or no.
                    inquiry: {userMessage.Contents.OfType<TextContent>()?.FirstOrDefault()}
                    professions: {professions}
                    """;
                var message = new ChatMessage() { 
                    Contents = [new TextContent(llmPrompt)]
                };
                var response = await context.ChatClient.GetResponseAsync(message);
                return response.Text.Contains("true");
            }
        }
    }
}
