using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Text;

namespace ClientManagement.AI
{
    public static class AgentExtensions
    {
        extension(AIAgent agent) {

            public async Task<string> HandleUserRequest( IList<AIContent> messageContents, IList<ChatMessage> ChatHistory, AgentSession? session = null, Action<Dictionary<string, string>>? assistantReasoningTokenHandler = null)
            {
                await Task.Yield();
                if (session is null)
                {
                    session = await (agent).CreateSessionAsync();
                }
                var userMessage = new ChatMessage(ChatRole.User, messageContents);
                IList<AIContent> chatContext = [.. ChatHistory.SelectMany(x => x.Contents), .. messageContents];
                ChatHistory.Add(userMessage);
                var response = (agent).RunStreamingAsync(new ChatMessage(ChatRole.User, chatContext), session);
                var agentResponse = string.Empty;
                var agentThoughts = new Dictionary<string, string>();
                var lastAgentToRespond = string.Empty;
                await foreach (var item in response)
                {
                    var toolApprovalRequestContent = item.Contents
                        .OfType<ToolApprovalRequestContent>().FirstOrDefault();
                    var reasoningContent = item.Contents.OfType<TextReasoningContent>().FirstOrDefault();
                    if (toolApprovalRequestContent is not null)
                    {
                        var toolResponse = toolApprovalRequestContent.CreateResponse(true);
                        var wrapped = new ChatMessage(ChatRole.User, [toolResponse]);
                        await foreach (var approvedItem in agent.RunStreamingAsync(wrapped, session))
                        { 
                            agentResponse += approvedItem.Text;
                            lastAgentToRespond = item.AuthorName;
                        }
                    }
                    if (reasoningContent is not null)
                    {
                        if (!agentThoughts.ContainsKey(item.AuthorName))
                        {
                            agentThoughts[item.AuthorName] = string.Empty;
                        }
                        agentThoughts[item.AuthorName] += reasoningContent.Text;
                        assistantReasoningTokenHandler?.Invoke(agentThoughts);
                        await Task.Yield();
                    }
                    agentResponse += item.Text;
                   
                }
                if (!string.IsNullOrWhiteSpace(agentResponse)) {
                    ChatHistory.Add(new ChatMessage(ChatRole.Assistant, [new TextContent(agentResponse)]));
                }
                return agentResponse;
            }
        }
    }
}
