using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OllamaSharp;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    internal class UserContentValidatorSelector : SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public UserContentValidatorSelector(UserAssistantContext context) : base(context)
        {
           Actions.Add(HasCorrectFileType);
           Actions.Add(TextContentIsValid);
        }

        public Task<ChatResponse> HasCorrectFileType(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            context.Logger.LogInformation($"executing {nameof(HasCorrectFileType)}");
            var isCorrectFileType = true;
            var fileContents = userMessage.Contents.OfType<DataContent>();
            foreach (var content in fileContents) {
                
                if (Regex.Match(content.MediaType, @"^(image/)", RegexOptions.IgnoreCase).Success) //image detected
                {
                    context.Logger.LogInformation($"image({content.MediaType}) detected");
                    continue;
                }

                if(!context.AgentOptions.Value.SupportedFileTypes.Select(x => x.ToLower()).Contains(content.MediaType.ToLower()))
                {
                    context.Logger.LogInformation($"invalid file type ({content.MediaType}) detected.");
                    isCorrectFileType = false;
                }
            }
            var responseMessage = !isCorrectFileType ? "Incorrect file type detected in user's message" : "";
            var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, responseMessage));
            response.AdditionalProperties = new AdditionalPropertiesDictionary() {
                [Constants.ExecutionStatus.Success] = isCorrectFileType 
            };
            if (!string.IsNullOrWhiteSpace(response.Text))
            {
                context.Logger.LogInformation($"Response from {nameof(HasCorrectFileType)}: "+response.Text);
            }
            return Task.FromResult(response);
        }

        public async Task<ChatResponse> TextContentIsValid(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            context.Logger.LogInformation($"executing {nameof(TextContentIsValid)}");
            var textContent = string.Join("\n",userMessage.Contents.OfType<TextContent>());
           
            var criteria = context.AgentOptions.Value.AiAgentMetaData.Select(x => new KeyValuePair<string, string>(x.Profession, x.Description ?? string.Empty))
                .ToDictionary(x => x.Key, x=> x.Value);
            SystemOneResponse? ans = null;
            if(context.ChatClient is IOllamaApiClient oApiClient)
            {
               ans = await oApiClient.SystemOneAsync(new()
                {
                    Model = context.AgentOptions.Value.SystemOneModel,
                    State = textContent,
                    Questions = {
                     ["label"] = new SystemOneChoiceQuestion()
                     {
                         Criteria = criteria,
                         Instructions = "Which profession is this in alignment with?"
                     }
                    }
                });
            }
            var systemOneChoiceResponse = (ans?.Answers?["label"] as SystemOneChoiceAnswer);
            var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, systemOneChoiceResponse?.Confidence >=0.6 ? systemOneChoiceResponse?.Choice:"This inquiry is outside the scope of my expertise."));
            response.AdditionalProperties = new AdditionalPropertiesDictionary()
            {
                [Constants.ExecutionStatus.Success] = !string.IsNullOrWhiteSpace(systemOneChoiceResponse?.Choice)

            };
            if (!string.IsNullOrWhiteSpace(response.Text))
            {
                context.Logger.LogInformation($"Response from {nameof(TextContentIsValid)}: {response.Text}");
            }
            return response;
        }
    }
}
