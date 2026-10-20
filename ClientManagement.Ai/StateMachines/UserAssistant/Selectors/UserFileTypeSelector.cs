using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class UserFileTypeSelector : SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public UserFileTypeSelector(UserAssistantContext context) : base(context)
        {
           Actions  = Actions.Append(HasCorrectFileType);
        }

        public Task<ChatResponse> HasCorrectFileType(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            var isCorrectFileType = true;
            var fileContents = userMessage.Contents.OfType<DataContent>();
            foreach (var content in fileContents) {

                if (Regex.Match(content.MediaType, @"^(image/)", RegexOptions.IgnoreCase).Success) //image detected
                {
                    continue;
                }

                if(!context.AgentOptions.Value.SupportedFileTypes.Select(x => x.ToLower()).Contains(content.MediaType.ToLower()))
                {
                    isCorrectFileType = false;
                }
            }
            var responseMessage = !isCorrectFileType ? "Incorrect file type detected in user's message" : "";
            var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, responseMessage));
            response.AdditionalProperties = new AdditionalPropertiesDictionary() {
                [Constants.ExecutionStatus.Success] = isCorrectFileType 
            };
            return Task.FromResult(response);
        }
    }
}
