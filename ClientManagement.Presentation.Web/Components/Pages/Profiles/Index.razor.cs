using Core.Presentation.Models.ViewModels;
using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
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
