using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Areas.Accounts.Pages.Base;
using Core.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Net.Http.Json;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class RegisterModel : PageModelBase<RegisterViewModel>
    {
        private readonly HttpClient _httpClient;
        public string Message { get; set; } = string.Empty;
        
        public string MessageColor => this.Message.Contains("success", StringComparison.OrdinalIgnoreCase) ? "success" : "danger";
        private readonly UserManager<IdentityUser> _userManager;
       // private readonly IUserProfileService _userProfileService;
        public RegisterModel(IHttpClientFactory httpClientFactory, UserManager<IdentityUser> userManager) { 
           this._httpClient = httpClientFactory.CreateClient("AppApi");
            this._userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task OnPostAsync(RegisterViewModel registration)
        {
            if (ModelState.IsValid)
            {
                var apiResponse = await this._httpClient.PostAsJsonAsync("/register", new Dictionary<string, string>
              {
                  { "email", registration.CompanyEmail },
                  { "password", registration.Password }
              });

                if (apiResponse is { IsSuccessStatusCode: true })
                {
                    this.Message = "Registration successful";
                    var newUser = _userManager.Users.FirstOrDefault(x => x.UserName == registration.CompanyEmail)!;
                    newUser.PhoneNumber = registration.CompanyPhone;
                    await _userManager.UpdateAsync(newUser);
                    var userProfile = new ProfileDto
                    {
                        Email = registration.CompanyEmail,
                        Phone = registration.CompanyPhone,
                        Name = registration.CompanyName,
                    };
                   var response =  await this._httpClient.PostAsJsonAsync($"{ApiEndPointConstants.CreateProfile}/{newUser.Id}", userProfile);
                    if (response.IsSuccessStatusCode
                        && await response.Content.ReadFromJsonAsync<ResponseDto<UserProfileDto>>() is ResponseDto<UserProfileDto> validResponse
                        )
                    {
                        this.Message = validResponse.Message;
                    }
                    else
                    {
                        this.Message = "Login and complete your profile";
                    }
                }
                else
                {
                    this.Message = "Registration failed";
                    this.ViewModel.CompanyPhone = registration.CompanyPhone;
                    this.ViewModel.CompanyName = registration.CompanyName;
                    this.ViewModel.CompanyEmail = registration.CompanyEmail;
                    this.ViewModel.Password = registration.Password;
                    this.ViewModel.ConfirmPassword = registration.ConfirmPassword;
                }
            }
            else { 
              this.Message = "Registration failed";
                this.ViewModel.CompanyPhone = registration.CompanyPhone;
                this.ViewModel.CompanyName = registration.CompanyName;
                this.ViewModel.CompanyEmail = registration.CompanyEmail;
                this.ViewModel.Password = registration.Password;
                this.ViewModel.ConfirmPassword = registration.ConfirmPassword;

            }

        }
    }
}
