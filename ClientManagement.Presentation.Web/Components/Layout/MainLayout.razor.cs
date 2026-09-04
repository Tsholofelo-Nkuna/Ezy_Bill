using ClientManagement.BusinessLogicLayer.Agents;
using ClientManagement.BusinessLogicLayer.Agents.Tools;
using ClientManagement.BusinessLogicLayer.Agents.Workflows;
using Core.Presentation.Models.ViewModels;
using Microsoft.Agents.AI;
using Microsoft.Agents.AI.Workflows;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.AI;
using System.Net.NetworkInformation;

namespace ClientManagement.Presentation.Web.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase
    {
        public MainLayoutViewModel ViewModel { get; set; } = new MainLayoutViewModel();
        public bool ShowUserProfileModal { get; set; }
        public string Username { get => this._httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? string.Empty;  }
        public string UserId { get; set; } = string.Empty;
        [Inject]
        private IHttpContextAccessor? _httpContextAccessor { get; set; }
        [Inject] UserManager<IdentityUser>? UserManager { get; set; }
        [Inject] NavigationManager? NavigationManager { get; set; }
        public IEnumerable<UserProfileModalListItem>UserProfileModalListItems { get; set; } = [ ];
        public bool AssistantLaunced { get; set; } = false;
        public string AgentInstructions { get; set; } = string.Empty;
        public (string contentType, byte[] contents, string dataUrl) ImagePresentedToAgent;
        public bool AgentIsBusy { get; set;  } = false;
        public string AgentResponse { get; set; } = string.Empty;
        [Inject]
        public BookkeepingAgent? BookkeeperAgent { get; set; }
        [Inject] AppAssistantWorkflowProvider? AppAssitantWorkFlowProviderInstance { get; set; }
        public AIAgent? AppAssistantAgent { get; set; }
        public List<ChatMessage> ChatHistory 
        {
            get;
            set;
        } = [];
        //public AgentSession ASession { get; set; }
        public Guid AgentConversationId { get; set; }
        public Guid KeyForImagePresentedToAgent = Guid.NewGuid();
        protected override async Task OnInitializedAsync()
        {
            //this.Username = this._httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
           // AgentWorkflowBuilder.
            AppAssistantAgent  =  AppAssitantWorkFlowProviderInstance?.Create()?.AsAIAgent(name:"Application assistant", description:"routes all user requests to the appropriate specialist");
            await base.OnInitializedAsync();
            if(UserManager.Users.FirstOrDefault(x => x.UserName == this.Username) is IdentityUser currentUser)
            {
                this.UserId = currentUser.Id;
            }
            this.UserProfileModalListItems = [
                 new(){ 
                     UrlPath = $"Profiles/{this.UserId}", 
                     Text = "Profile",
                     IconClass = "bi bi-person-lines-fill me-1" 
                 },
            ];
        }

        public async Task OnFilePresentedToAgent(InputFileChangeEventArgs e)
        {
            using var fileStream = e.File.OpenReadStream();
            if(fileStream is not null)
            {
                var bytes = new byte[fileStream.Length];
                var memory = bytes.AsMemory(0, (int)fileStream.Length);
                await fileStream.ReadExactlyAsync(memory);
                var contentType = e.File.ContentType;
                //if (contentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)) 
                //{
                //    contentType = "image/jpg";
                //}
                var dataUrl = $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";
                ImagePresentedToAgent = (e.File.ContentType, bytes, dataUrl);
            }
            else
            {
                //ImagePresentedToAgent = string.Empty;
            }
           
        }

        public void OnUserProfileModalItemClicked(UserProfileModalListItem item)
        {
            this.ShowUserProfileModal = false;
            this.NavigationManager.NavigateTo(item.UrlPath);
        }

        public Task OnClearAgentConversation()
        {
           
            AgentInstructions = string.Empty;
            BookkeeperAgent?.ChatHistory?.Clear();
            ImagePresentedToAgent = default;
            return Task.CompletedTask;
        }

        public async Task OnAgentIconClicked()
        {
            AssistantLaunced = true;
            AgentConversationId = Guid.NewGuid();
            //ASession = await AppAssistantAgent.CreateSessionAsync();
           // return Task.CompletedTask;
        }
        public async Task OnSendAgentInstructions()
        {
            AgentIsBusy = true;
            AgentResponse = string.Empty;
            await Task.Yield();
            IList<AIContent> messageContents = [new TextContent(AgentInstructions)];
            
            if (!string.IsNullOrWhiteSpace(ImagePresentedToAgent.contentType))
            {
                messageContents.Add(new DataContent(ImagePresentedToAgent.dataUrl));
            }
            
            AgentResponse = await AgentBase.HandleUserRequest(AppAssistantAgent,messageContents, BookkeeperAgent.ChatHistory) ?? string.Empty;
            
            AgentIsBusy = false;
            AgentInstructions = string.Empty;
            ImagePresentedToAgent = default;
            KeyForImagePresentedToAgent = Guid.NewGuid();
        }
    }
     
    public class UserProfileModalListItem
    {
        public string UrlPath { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
    }
}
