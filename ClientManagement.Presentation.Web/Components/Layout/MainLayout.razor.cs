using Core.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using System.Net.NetworkInformation;

namespace ClientManagement.Presentation.Web.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase
    {
        public MainLayoutViewModel ViewModel { get; set; } = new MainLayoutViewModel();
        public bool ShowUserProfileModal { get; set; }

        public string Username { get => this._httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;  }
        public string UserId { get; set; } = string.Empty;
        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }
        [Inject] UserManager<IdentityUser> UserManager { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }
        public IEnumerable<UserProfileModalListItem>UserProfileModalListItems { get; set; } = [ ];
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
    }
     
    public class UserProfileModalListItem
    {
        public string UrlPath { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string IconClass { get; set; } = string.Empty;
    }
}
