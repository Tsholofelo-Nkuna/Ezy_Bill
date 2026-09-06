using ClientManagement.Models.ViewModels;
using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;

namespace Core.Presentation.ViewComponents.Components.Pages.Profiles
{
    public partial class IndexPage : GenericComponentBase<ProfileViewModel, UserProfileDto>
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
            get => this.ViewModel.UserProfileFormViewModel.ViewModelState.FirstOrDefault() ?? new UserProfileDto();
            set
            {
                this.ViewModel.UserProfileFormViewModel.ViewModelState = [value];
                StateHasChanged();
            }
        }

        public async Task GetData()
        {
            var userProfileResponse = await this.AppApi.GetFromJsonAsync<ResponseDto<UserProfileDto>>($"{this.BaseUrl}/{this.UserId}");
            if(userProfileResponse is { Data: UserProfileDto} validUserProfile)
            {
                this.UserProfile = validUserProfile.Data;
               
            }
        }

        public async Task OnUserProfileDataSubmit(EventState<IEnumerable<UserProfileDto>> eventState)
        {
            if (eventState.Success && eventState.Payload.FirstOrDefault() is UserProfileDto submittedRecord)
            {
               var response = await this.AppApi.PostAsJsonAsync($"{this.BaseUrl}/{this.UserProfile.User.Id}", submittedRecord.Profile);
                if(response is { IsSuccessStatusCode : true } successResponse)
                {
                    var result = await successResponse.Content.ReadFromJsonAsync<ResponseDto<UserProfileDto>>();
                    if(result is { Data : UserProfileDto } successResult && successResult.Data.User.Id == this.UserProfile.User.Id)
                    {
                        await this.GetData();
                    }
                }
            }
        }
    }
}
