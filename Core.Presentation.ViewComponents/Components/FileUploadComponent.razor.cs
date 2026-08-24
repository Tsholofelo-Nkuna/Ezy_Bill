using Core.Presentation.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class FileUploadComponent
    {
        [Parameter]
        public string InputId { get; set; } = Guid.NewGuid().ToString();

        [Parameter]
        public string Label { get; set; } = "Upload File";

        [Parameter]
        public string AcceptedFileTypes { get; set; } = ".pdf"; // e.g., ".pdf,.doc,.docx" or "image/*"

        [Parameter]
        public long MaxFileSize { get; set; } = 5242880; // 5 MB default

        [Parameter]
        public bool AllowMultiple { get; set; } = false;

        [Parameter]
        public EventCallback<List<IBrowserFile>> OnMultipleFilesSelected { get; set; }

        [Parameter]
        public string? CssClass { get; set; }

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        /// <summary>
        /// List of uploaded files that the parent component can control
        /// </summary>
        [Parameter]
        public List<AppFile> UploadedFiles { get; set; } = new List<AppFile>();

        [Parameter]
        public EventCallback<List<(string fileName, long fileSize, Guid fileId)>> UploadedFilesChanged { get; set; }

        protected string FileInputId => $"file-input-{InputId}";
        protected string? ErrorMessage { get; set; }
        protected string? SuccessMessage { get; set; }

        /// <summary>
        /// Handles file selection event and appends new files to UploadedFiles
        /// </summary>
        protected async Task OnFileInputChanged(InputFileChangeEventArgs e)
        {
            ErrorMessage = null;
            SuccessMessage = null;
            var newFiles = new List<IBrowserFile>();

            try
            {
                if (AllowMultiple)
                {
                    foreach (var file in e.GetMultipleFiles())
                    {
                        if (ValidateFile(file))
                        {
                            newFiles.Add(file);
                        }
                    }

                    if (newFiles.Any())
                    {
                       // UploadedFiles.AddRange(newFiles);
                        //SuccessMessage = $"{newFiles.Count} file(s) uploaded successfully.";
                        await UploadedFilesChanged.InvokeAsync(newFiles.Select(x => ((x.Name, x.Size, Guid.Empty ))).ToList());
                        await OnMultipleFilesSelected.InvokeAsync(newFiles);
                    }
                }
                else
                {
                    var file = e.File;
                    if (ValidateFile(file))
                    {
                        newFiles.Add(file);
                        //UploadedFiles.Add(file);
                       // SuccessMessage = $"File '{file.Name}' uploaded successfully.";
                        await UploadedFilesChanged.InvokeAsync(newFiles.Select(x => ((x.Name, x.Size, Guid.Empty))).ToList());
                        await OnMultipleFilesSelected.InvokeAsync([file]);
                    }
                }
            }
            catch (Exception ex)
            {
                //ErrorMessage = $"Error processing file: {ex.Message}";
            }
        }

        /// <summary>
        /// Validates file based on size and type constraints
        /// </summary>
        private bool ValidateFile(IBrowserFile file)
        {
            // Validate file size
            if (file.Size > MaxFileSize)
            {
                ErrorMessage = $"File '{file.Name}' exceeds maximum size of {MaxFileSize / 1024 / 1024} MB.";
                return false;
            }

            // Validate file type
            if (!string.IsNullOrEmpty(AcceptedFileTypes))
            {
                var acceptedTypes = AcceptedFileTypes.Split(',').Select(t => t.Trim()).ToList();
                var isValidType = acceptedTypes.Any(acceptedType =>
                {
                    if (acceptedType == "image/*" && file.ContentType.StartsWith("image/"))
                        return true;
                    if (acceptedType == "video/*" && file.ContentType.StartsWith("video/"))
                        return true;
                    if (acceptedType == "audio/*" && file.ContentType.StartsWith("audio/"))
                        return true;
                    return file.Name.EndsWith(acceptedType) || file.ContentType == acceptedType;
                });

                if (!isValidType)
                {
                    ErrorMessage = $"File type not accepted. Accepted types: {AcceptedFileTypes}";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Removes a file from the uploaded files list
        /// </summary>
        public async Task RemoveFile(AppFile file)
        {
           //Fire remove event
        }

        /// <summary>
        /// Clears all uploaded files and messages
        /// </summary>
        public async Task ClearAllFiles()
        {
            
        }

        /// <summary>
        /// Gets the current list of uploaded files
        /// </summary>
        public List<IBrowserFile> GetUploadedFiles()
        {
            return [];
        }
    }
}
