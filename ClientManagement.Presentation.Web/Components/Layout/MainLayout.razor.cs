using ClientManagement.BusinessLogicLayer.Agents;
using Core.Presentation.Models.ViewModels;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Components;
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
        public bool AgentIsBusy { get; set;  } = false;
        public string AgentResponse { get; set; } = string.Empty;
        [Inject]
        public BookkeepingAgent? Bookkeeper { get; set; }
        public ChatClientAgent? BookkeeperAgent 
        { 
            get
            {
                Bookkeeper?.Name = "CEO_Judie";
                return Bookkeeper?.AgentInstance;
            } 
        }

        public List<ChatMessage> ChatHistory 
        {
            get;
            set;
        } = [];
        public Guid AgentConversationId { get; set; }
        protected override async Task OnInitializedAsync()
        {
            //this.Username = this._httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
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

        public void OnUserProfileModalItemClicked(UserProfileModalListItem item)
        {
            this.ShowUserProfileModal = false;
            this.NavigationManager.NavigateTo(item.UrlPath);
        }

        public Task OnClearAgentConversation()
        {
           
            AgentInstructions = string.Empty;
            this.ChatHistory.Clear();
            return Task.CompletedTask;
        }

        public Task OnAgentIconClicked()
        {
            AssistantLaunced = true;
            AgentConversationId = Guid.NewGuid();
            return Task.CompletedTask;
        }
        public async Task OnSendAgentInstructions()
        {
            AgentIsBusy = true;
            AgentResponse = string.Empty;
            await Task.Yield();
            
            var session = await BookkeeperAgent.CreateSessionAsync();
            var userMessage = new ChatMessage(ChatRole.User, [new TextContent(AgentInstructions)]);
            IList<AIContent> chatContext = [.. this.ChatHistory.Select(x => new TextContent(x.Text)), new TextContent(userMessage.Text)];
            this.ChatHistory.Add(userMessage);
            var response = BookkeeperAgent.RunStreamingAsync(new ChatMessage(ChatRole.User, chatContext), session);
            await foreach (var item in response)
            {

                var toolApprovalRequestContent = item.Contents
                    .OfType<ToolApprovalRequestContent>().FirstOrDefault();
                if (toolApprovalRequestContent is not null)
                {
                    var toolResponse = toolApprovalRequestContent.CreateResponse(true);
                    var wrapped = new ChatMessage(ChatRole.User, [toolResponse]);
                    await foreach (var approvedItem in BookkeeperAgent.RunStreamingAsync(wrapped, session))
                    {
                        AgentResponse += approvedItem.Text;
                    }
                }
                else
                {
                    AgentResponse += item.Text;
                }

            }
            this.ChatHistory.Add(new ChatMessage(ChatRole.Assistant, [new TextContent(AgentResponse)]));
            AgentIsBusy = false;
            AgentInstructions = string.Empty;
        }
    }
     
    public class UserProfileModalListItem
    {
        public string UrlPath { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
    }
}
