using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ClientManagement.Presentation.Web.Areas.Admin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfilesViewModel UserProfilesViewModel { get; set; } = new();
        public IndexModel(IUserProfileService userProfileService) {
          this._userProfileService = userProfileService;
        }
        public async Task OnGetAsync()
        {
            this.UserProfilesViewModel.UserProfileTableViewModel.ViewModelState = await _userProfileService.GetAllProfiles();
        }
    }
}
