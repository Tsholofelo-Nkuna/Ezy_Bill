using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Presentation.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.RazorPages;


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
