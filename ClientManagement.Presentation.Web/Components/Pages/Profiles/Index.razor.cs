using ClientManagement.Presentation.Models.Profiles;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Profiles
{
    public partial class Index : GenericComponentBase<ProfileViewModel, UserProfileDto>
    {
        [Parameter]
        public string UserId { get; set; } = string.Empty;
        protected override async void OnInitialized()
        {
            base.OnInitialized();
            this.BaseUrl = "api/UserProfiles";
            await this.GetData();
        }

        public UserProfileDto UserProfile 
        { 
            get => this.ViewModel.ViewModelState.FirstOrDefault() ?? new UserProfileDto();
            set
            {
                this.ViewModel.ViewModelState = [value];
                StateHasChanged();
            }
        }

        public async Task GetData()
        {
            var userProfileResponse = await this.AppApi.GetFromJsonAsync<UserProfileDto>($"{this.BaseUrl}/{this.UserId}");
            if(userProfileResponse is UserProfileDto validUserProfile)
            {
                this.UserProfile = validUserProfile;
               
            }
           
        }
    }
}
