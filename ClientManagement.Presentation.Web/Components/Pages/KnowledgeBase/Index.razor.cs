using ClientManagement.Presentation.Web.Controllers;
using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects;
using Core.Utils.Constants;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Web.Components.Pages.Files
{
    public partial class Index
    {
        private List<AppFile> UploadedFiles = new();
        [Inject]
        private IHttpClientFactory _httpClientFactory { get; set; }
        public IList<AppFileDto> SelectedFiles { get; set; } = [];
        public bool FileSubmissionInProgress { get; set; } = false;
        public IEnumerable<(string name, string displayName, string agentName)> VectoreStoreNames { get; set; } = [];
        private string? SelectedVectoreStoreName { get; set; }
        public HttpClient AppApi
        {
            get
            {
                var httpC = _httpClientFactory.CreateClient("AppApi");

               // httpC.DefaultRequestHeaders.Add(AuthConstants.XApiKey, $"{this.CurrentUser.Identity.Name}");
                return httpC;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetData();
        }
        
        public async Task GetData()
        {
            var response = await this.AppApi.PostAsJsonAsync<PageRequestDto<AppFileDto>>("/api/AppFile/Get", new()
            {
                GetAllPages = true
            });
            if (response is { StatusCode: System.Net.HttpStatusCode.OK } && response.Content.ReadFromJsonAsync<PageResponseDto<AppFileDto>>().Result is PageResponseDto<AppFileDto> serverResponse)
            {
                UploadedFiles = serverResponse.Items.Select(x => (new AppFile()
                {
                    FileName = x.FileName,
                    FileId = x.Id,
                    MimeType = x.MimeType,
                    Contents = x.Contents,
                    FileSize = x.FileSize,
                })).ToList();
            }
            
            var vectoreStoreNameResponse = await this.AppApi.GetFromJsonAsync<IEnumerable<Dictionary<string, string>>>("/api/AppFile/VectoreStoreNames");
            this.VectoreStoreNames = vectoreStoreNameResponse?.Select(x =>
            {
                x.TryGetValue("Name", out var name);
                x.TryGetValue("DisplayName", out var displayName);
                x.TryGetValue("AgentName", out var agentName);
                return (name, displayName, agentName);
            }) ?? [];
        }

        public async Task OnSubmitFiles()
        {
           if(SelectedFiles.Any() && !string.IsNullOrWhiteSpace(this.SelectedVectoreStoreName))
            {
                FileSubmissionInProgress = true;
                await Task.Yield();
                var response = await this.AppApi.PostAsJsonAsync($"api/AppFile/{this.SelectedVectoreStoreName}", SelectedFiles);

                if (response is { StatusCode: System.Net.HttpStatusCode.OK } && response.Content.ReadFromJsonAsync<bool>().Result is bool filesSubmitted)
                {
                    if (filesSubmitted)
                    {
                        await GetData();
                    }
                }
                FileSubmissionInProgress = false;
            }
        }
        /// <summary>
        /// Handles the file selection event from FileUploadComponent
        /// Processes the selected files and updates the UI
        /// </summary>
        private  async Task HandleFilesSelected(List<IBrowserFile> files)
        {
            try
            {
                SelectedFiles = [];
                foreach(var file in files)
                {
                    using var fileStream = file.OpenReadStream();
                    var fileContents = new byte[fileStream.Length];
                    await fileStream.ReadExactlyAsync(fileContents.AsMemory(0, (int)fileStream.Length));
                   SelectedFiles.Add( new AppFileDto
                    {
                        FileName = file.Name,
                        MimeType = file.ContentType,
                        Contents = fileContents,
                        FileSize = file.Size,
                        AgentName = this.VectoreStoreNames.FirstOrDefault(x => x.name.Equals(this.SelectedVectoreStoreName, StringComparison.OrdinalIgnoreCase)).agentName
                    });
                }
            }
            catch (Exception)
            {

                throw;
            }

          
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
