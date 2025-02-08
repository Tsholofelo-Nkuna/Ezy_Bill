using ClientManagement.Presentation.Models;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;

namespace ClientManagement.Presentation.Web.Components.Layout
{
    public partial class MainLayout: LayoutComponentBase
    {
        public MainLayoutViewModel ViewModel { get; set; } = new MainLayoutViewModel();
        public bool ShowUserProfileModal { get; set; }

        public string Username { get => this._httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;  }

        [Inject]
        private IHttpContextAccessor _httpContextAccessor { get; set; }

        protected override Task OnInitializedAsync()
        {
            //this.Username = this._httpContextAccessor.HttpContext?.User?.Identity?.Name ?? string.Empty;
            return base.OnInitializedAsync();
        }
    }
}
