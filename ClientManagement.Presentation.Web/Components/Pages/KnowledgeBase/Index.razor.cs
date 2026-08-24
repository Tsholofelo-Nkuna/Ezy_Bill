using Core.Presentation.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Web.Components.Pages.Files
{
    public partial class Index
    {
        private List<AppFile> UploadedFiles = new();
        /// <summary>
        /// Handles the file selection event from FileUploadComponent
        /// Processes the selected files and updates the UI
        /// </summary>
        private  Task HandleFilesSelected(List<IBrowserFile> files)
        {
           
            foreach (var file in files)
            {
               

                // TODO: Add the files to Qdrant vector store
                // In the business logic layer project, there is an Agents directory which containt a DocumentToolKit; u
                // use it to break down the pdf document into manageable chunks, which will in turn be converted to embeddings to be fed to the vector store
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Formats the total size of uploaded files to a readable format
        /// </summary>
        private string FormatTotalSize()
        {
            long totalBytes = UploadedFiles.Sum(f => f.FileSize);
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = totalBytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}
