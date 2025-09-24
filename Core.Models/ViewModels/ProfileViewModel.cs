using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects;


namespace Core.Presentation.Models.ViewModels
{
    public class ProfileViewModel : GenericListViewModel<UserProfileDto>
    {
        public ProfileViewModel(): this([new()])
        {
        }
        public ProfileViewModel(IEnumerable<UserProfileDto> state) : base(state)
        {
        }

        public FormComponentViewModel<UserProfileDto> UserProfileFormViewModel { get; set; } = new([new()], "UserProfileForm")
        {
            ColClass = "col-12",
            Fields = [
                   new(nameof(UserProfileDto.ProfileName), "Name/Company Name") {
                      
                       ControlType = ControlType.Text,
                   },
                  new(nameof(UserProfileDto.ProfileEmail), "Email/Company email") {
                     
                       ControlType = ControlType.Email,
                   },
                   new(nameof(UserProfileDto.ProfilePhone), "Phone/Company phone") {
                       ControlType = ControlType.Text,
                   },
                ],
            ControllerName = "UserProfiles",
            
        };

        public TabsComponentViewModel<UserProfileDto> UserProfileTabsViewModel { get; set; } = new([new()])
        {
            TabItems = [
                new("Details","User-Profile-Details", "Details"){
                    Active = true,
                }
            ]

        };
    }
}
