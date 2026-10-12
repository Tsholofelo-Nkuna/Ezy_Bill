using ClientManagement.Ai.Tests.TestData;
using ClientManagement.AI.StateMachines.UserAssistant;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClientManagement.Ai.Tests
{
    internal class UserAssistantChatTests
    {
        internal UserAssitantContext UAssistantContext { get; set; }
        internal EventHandler<ChatMessage> UserMessageSignal { get; set; }
        internal UserAssistantChatTests() {
            var chatClientMoq = new Mock<IChatClient>();
            var agentOptionsMoq = new Mock<IOptions<AgentOptions>>();
            agentOptionsMoq.Setup(x => x.Value).Returns(new AgentOptions());
            UAssistantContext = new UserAssitantContext(chatClientMoq.Object, agentOptionsMoq.Object);
            UserMessageSignal += UAssistantContext.;
            UAssistantContext.HtmlInputFormOutputSignal += HtmlFormOutputListener;
        }

        [Theory, ClassData<UserAssistantChatTestData>]
        internal void ChatAssistant_Works(string userMessage, AgentOptions options, string expected)
        {
            
            while(!userMessage.Equals("exit", StringComparison.OrdinalIgnoreCase))
            {
                UserMessageSignal.Invoke(null, new(ChatRole.User, userMessage));
               
            }
        }

       internal void HtmlFormOutputListener(object? sender, string htmlForm)
       {
            var isHtml = Regex.Match(htmlForm, @"(<[a-z]>.*</[a-z]>|[^\s]{1,})", RegexOptions.IgnoreCase).Success;
            Assert.True(isHtml, !isHtml ? "No html content found!" : string.Empty);
       }
    }
}
