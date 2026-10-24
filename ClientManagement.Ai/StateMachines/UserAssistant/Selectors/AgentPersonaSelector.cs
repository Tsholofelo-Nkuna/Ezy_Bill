using Azure;
using ClientManagement.Ai.Helpers;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.Models.AI;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OllamaSharp.Models;
using System.ComponentModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    [Description("Determines which agent persona to load based on the user's inquiry")]
    internal class AgentPersonaSelector : SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public AgentPersonaSelector(UserAssistantContext context) : base(context)
        {
           Actions.Add(HasCorrectFileType);
           Actions.Add(TextContentIsValid);
           Actions.Add(LoadAgentPersona);
        }

        public Task<ChatResponse> HasCorrectFileType(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            try
            {
                context.Logger.LogInformation($"executing {nameof(HasCorrectFileType)}");
                var isCorrectFileType = true;
                var fileContents = userMessage.Contents.OfType<DataContent>();
                foreach (var content in fileContents)
                {

                    if (Regex.Match(content.MediaType, @"^(image/)", RegexOptions.IgnoreCase).Success) //image detected
                    {
                        context.Logger.LogInformation($"image({content.MediaType}) detected");
                        continue;
                    }

                    if (!context.AgentOptions.Value.SupportedFileTypes.Select(x => x.ToLower()).Contains(content.MediaType.ToLower()))
                    {
                        context.Logger.LogInformation($"invalid file type ({content.MediaType}) detected.");
                        isCorrectFileType = false;
                    }
                }
                var responseMessage = !isCorrectFileType ? "Incorrect file type detected in user's message" : "";
                var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, responseMessage));
                response.AdditionalProperties = new AdditionalPropertiesDictionary()
                {
                    [Constants.ExecutionStatus.Success] = isCorrectFileType
                };
                if (!string.IsNullOrWhiteSpace(response.Text))
                {
                    context.Logger.LogInformation($"Response from {nameof(HasCorrectFileType)}: " + response.Text);
                }
                return Task.FromResult(response);
            }
            catch (Exception ex)
            {
                context.Logger.LogInformation(ex, $"{ex.StackTrace}");
                throw;
            }
        }

        public async Task<ChatResponse> TextContentIsValid(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            try
            {
                
                context.Logger.LogInformation($"executing {nameof(TextContentIsValid)} from {nameof(AgentPersonaSelector)}");
                var textContent = string.Join("\n", userMessage.Contents.OfType<TextContent>());
                var critetira = context.AgentOptions.Value.AiAgentMetaData.Where(x => x is { VecStoreMetaData: { Name : { Length : > 0} } }).Select(x => new KeyValuePair<string, string?>(x.VecStoreMetaData!.Name, x.VecStoreMetaData?.Description))
                    .ToDictionary();
                
                var systemOneResponse = await context.ChatClient.SystemOneChoiceQuestion(textContent, critetira);
                var systemOnChoiceAnswer = systemOneResponse?.Answers?["label"] as SystemOneChoiceAnswer;
                context.Logger.LogInformation($"system one response: Choice = {systemOnChoiceAnswer?.Choice}, Confidence = {systemOnChoiceAnswer?.Confidence}");
                
                var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, !string.IsNullOrWhiteSpace(systemOnChoiceAnswer?.Choice) ? systemOnChoiceAnswer!.Choice : "Your inquiry is outside the scope of my expertise"));
                response.AdditionalProperties = new AdditionalPropertiesDictionary()
                {
                    [Constants.ExecutionStatus.Success] = !string.IsNullOrWhiteSpace(systemOnChoiceAnswer?.Choice) && context.AgentOptions.Value.AiAgentMetaData
                    .Select(x => x.VecStoreMetaData?.Name?.ToLower()).Where(x => !string.IsNullOrWhiteSpace(x)).Contains(systemOnChoiceAnswer?.Choice?.ToLower())

                };
                context.Logger.LogInformation($"additional response properties: {JsonSerializer.Serialize(response.AdditionalProperties)}");
                return response;
            }
            catch (Exception ex)
            {
                context.Logger.LogError(ex, $"{ex.StackTrace} {ex.InnerException}");
                throw;
            }
        }

        public async Task<ChatResponse> LoadAgentPersona(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            try
            {
                context.Logger.LogInformation($"executing {nameof(LoadAgentPersona)} from {nameof(AgentPersonaSelector)}");
                if (formerResponse?.AdditionalProperties?.TryGetValue(Constants.ExecutionStatus.Success, out bool successStatus) ?? false)
                {
                    if (successStatus)
                    {
                        context.State?.Persona = context.AgentOptions.Value.AiAgentMetaData?.Where(x => x.VecStoreMetaData?.Name.Equals(formerResponse.Text, StringComparison.OrdinalIgnoreCase) ?? false)?.FirstOrDefault();
                    }
                }
                ChatResponse response = new(new ChatMessage(ChatRole.Assistant, (context.State?.Persona is AgentMetaData aPersona) ? $"Loaded {aPersona.Name} persona" : "No agent persona loaded"));
                response.AdditionalProperties =  new AdditionalPropertiesDictionary(){
                    [Constants.ExecutionStatus.Success] = context.State?.Persona is not null
                };
                context.Logger.LogInformation($"Response text from {nameof(LoadAgentPersona)}: {response.Text}");
                return response;
            }
            catch (Exception ex)
            {
                context.Logger.LogError(ex, $"{ex.StackTrace}");
                throw;
            }
        }
    }
}
