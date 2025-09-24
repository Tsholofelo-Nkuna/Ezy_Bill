using Core.Presentation.Models.Base;
using Core.Presentation.Models.DataTransferObjects;


namespace Core.Presentation.Models.ViewModels
{
    public class UserProfilesViewModel : GenericListViewModel<UserProfileDto>
    {
        public UserProfilesViewModel() : this([])
        {
            
        }
        public UserProfilesViewModel(IEnumerable<UserProfileDto> state) : base(state)
        {
        }

        public TableComponentViewModel<UserProfileDto> UserProfileTableViewModel { get; set; }
        = new([])
        {
            ColumnConfigs = [
                new() {
                    Index = nameof(UserProfileDto.Username),
                    Name = "Username",
                },
                 new() {
                    Index = nameof(UserProfileDto.ProfileEmail),
                    Name = "Profile Email",
                },
                 new() {
                    Index = nameof(UserProfileDto.ProfilePhone),
                    Name = "Profile Phone",
                },
                 new() {
                    Index = nameof(UserProfileDto.ProfileId),
                    Name = "Profile Id",
                }
            ],
           ShowArchiveButton = false,
           ShowDeleteButton = false,
           ShowViewButton = false
            
        };
    }
}
