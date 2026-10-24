using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using OllamaSharp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
//using ChatResponse = Microsoft.Extensions.AI.ChatResponse;
//using ChatMessage = Microsoft.Extensions.AI.ChatMessage;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    [Description("Partitions content from the user based on the content's type")]
    internal class DataPartitionSelector: SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public DataPartitionSelector(UserAssistantContext context) : base(context)
        {
            Actions.Add(ClassifyContent);
        }

        public async Task<ChatResponse?> ClassifyContent(UserAssistantContext context, ChatMessage chatMessage, ChatResponse? formerChatResponse)
        {
            context.Logger.LogInformation($"executing {nameof(ClassifyContent)} from {nameof(DataPartitionSelector)}");
            var contentTypes = chatMessage.Contents.OfType<DataContent>()
                .ToList();
            Dictionary<string, string?> criteria = new()
            {
                [Constants.FileCategories.Images] = null,
                [Constants.FileCategories.WordDocuments] = null,
                [Constants.FileCategories.PdfDocuments] = null,
            };

            var classes = criteria.Select(x => new KeyValuePair<string, IList<DataContent>>(x.Key, [])).ToDictionary();
            foreach(var content in contentTypes)
            {
                var choiceAns = ((await context.ChatClient.SystemOneChoiceQuestion(content.MediaType, criteria))?.Answers["label"]) as SystemOneChoiceAnswer;
                if (classes.ContainsKey(choiceAns?.Choice))
                {
                    classes[choiceAns?.Choice].Add(content);
                }
            }
            var responseMessage = $"{classes.Count()} partitions created. {string.Join("\n",classes.Select(x => $"{x.Key} has {x.Value.Count()} entries"))}";
            context.Logger.LogInformation(responseMessage);
            var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, responseMessage));
            response.AdditionalProperties = new AdditionalPropertiesDictionary()
            {
                [Constants.ExecutionStatus.Success] = true
            };

            context.DataPartitions[Constants.FileCategories.PdfDocuments] = classes[Constants.FileCategories.PdfDocuments];
            context.DataPartitions[Constants.FileCategories.WordDocuments] = classes[Constants.FileCategories.WordDocuments];
            context.DataPartitions[Constants.FileCategories.Images] = classes[Constants.FileCategories.Images];
            return response;
        }
    }
}
