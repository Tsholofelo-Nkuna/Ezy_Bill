using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Base;
using ClientManagement.AI.StateMachines.UserAssistant.Selectors.Interfaces.Base;
using ClientManagement.Utils;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using UglyToad.PdfPig;

namespace ClientManagement.AI.StateMachines.UserAssistant.Selectors
{
    [Description("Decides which actions to perform on documents/images found within a user's request")]
    internal class DocumentProcessorSelector : SequentialSelectorBase, ISelectorBase<UserAssistantContext, ChatMessage>
    {
        public DocumentProcessorSelector(UserAssistantContext context) : base(context)
        {
            Actions.Add(ImageProcessor);
            Actions.Add(PdfProcessor);
            Actions.Add(WordProcessor);
        }

        public async Task<ChatResponse?> ImageProcessor(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            try
            {
                context.Logger.LogInformation($"executing {nameof(ImageProcessor)} from {nameof(DocumentProcessorSelector)}");
                if (context.DataPartitions?.TryGetValue(Constants.FileCategories.Images, out IList<DataContent> images) ?? false)
                {
                    var lmPrompt = $"""
                     Given the below, extract relevent information from the image(s) if possible, then construct a draft on how 
                     to resolve the user's inquiry using information gathered from the image(s). 
                     #Instructions:
                     {context.Persona.Instructions}
                     #User's inquiry:
                     {userMessage.Text}
               
                     #Rules:
                     - If no image(s) are available, don't bother with producing a draft.
                     - If image(s) don't contain relevant information, don't bother producing a draft.
                     - Avoid over thinking and producing lengthy drafts, make your drafts as short as possible; if possible limit them to {context.State.DraftSize} words.
                     - If no draft can be produced, respond with an empty string.
                    """;
                    context.Drafts[Constants.FileCategories.Images] = await context.ChatClient.GenerateResponseAsync(lmPrompt, images.Select(x=> x.Base64Data.ToString()));
                }

                var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, context.Drafts[Constants.FileCategories.Images]));
                response.AdditionalProperties = new AdditionalPropertiesDictionary()
                {
                    [Constants.ExecutionStatus.Success] = true
                };
                context.Logger.LogInformation($"response from {nameof(ImageProcessor)}: {response.Text}");
                return response;
            }
            catch (Exception ex)
            {
                context.Logger.LogInformation(ex, $"{ex.StackTrace}");
                throw;
            }
        }

        public async Task<ChatResponse?> PdfProcessor(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            try
            {
                context.Logger.LogInformation($"executing {nameof(PdfProcessor)} from {nameof(DocumentProcessorSelector)}");
                if (formerResponse?.AdditionalProperties?.TryGetValue(Constants.FileCategories.PdfDocuments, out IList<DataContent> pdfs) ?? false)
                {
                    var pdfContents = pdfs.Select(x => Convert.FromBase64String(x.Base64Data.ToString())).ToList();
                    var pdfText = string.Join("\n\n", pdfContents.Select(pdfContent =>
                    {
                        using var doc = UglyToad.PdfPig.PdfDocument.Open(pdfContent);
                        return string.Join("\n\n", doc.GetPages().Select(x => x.Text));
                    }));
                    var lmPrompt = $"""
                     Given the below, extract relevent information from the context if possible, then construct a draft on how 
                     to resolve the user's inquiry using information gathered from the context. 
                     #Instructions:
                     {context.Persona.Instructions}
                     #User's inquiry:
                     {userMessage.Text}
               
                     #Rules:
                     - If no pdf(s) are available, don't bother with producing a draft.
                     - If the context doesn't contain relevant information, don't bother producing a draft.
                     - Avoid over thinking and producing lengthy drafts, make your drafts as short as possible; if possible limit them to {context.State.DraftSize} words.
                     - If no draft can be produced, respond with an empty string.

                     #Context:
                     {pdfText}
                    """;

                    context.Drafts[Constants.FileCategories.PdfDocuments] = await context.ChatClient.GenerateResponseAsync(lmPrompt);
                }

                var response = new ChatResponse(new ChatMessage(ChatRole.Assistant, context.Drafts[Constants.FileCategories.PdfDocuments]));
                response.AdditionalProperties = new AdditionalPropertiesDictionary()
                {
                    [Constants.ExecutionStatus.Success] = true
                };
                context.Logger.LogInformation($"response from {PdfProcessor}: {response.Text}");
                return response;
            }
            catch (Exception ex)
            {
                context.Logger.LogInformation(ex, $"{ex.StackTrace}");
                throw;
            }
        }

        public Task<ChatResponse?> WordProcessor(UserAssistantContext context, ChatMessage userMessage, ChatResponse? formerResponse)
        {
            if (formerResponse?.AdditionalProperties?.TryGetValue(Constants.FileCategories.WordDocuments, out IList<DataContent> images) ?? false)
            {
                var lmPrompt = $"""
                     Given the below, extract relevent information from the word documents if possible, then construct a draft on how 
                     to resolve the user's inquiry using information gathered from the word document(s). 
                     #Instructions:
                     {context.Persona.Instructions}
                     #User's inquiry:
                     {userMessage.Text}
               
                     #Rules:
                     - If no word document(s) are available, don't bother with producing a draft.
                     - If word document(s) don't contain relevant information, don't bother producing a draft.
                     - Avoid over thinking and producing lengthy drafts, make your drafts as short as possible; if possible limit them to {context.State.DraftSize} words.
                     - If no draft can be produced, respond with an empty string.
                    """;
            }

            return Task.FromResult(formerResponse);
        }
    }
}
