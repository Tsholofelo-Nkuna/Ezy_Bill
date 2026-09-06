using ClientManagement.Models.ViewModels;
using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Profiles
{
    public partial class Index : GenericComponentBase<ProfileViewModel, UserProfileDto>
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
    }
}
