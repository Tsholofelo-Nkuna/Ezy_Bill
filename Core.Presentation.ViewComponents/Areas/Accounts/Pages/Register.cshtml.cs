using Core.Presentation.Models;
using Core.Presentation.ViewComponents.Areas.Accounts.Pages.Base;
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
        public string ConfirmPassword { get; set; } = string.Empty;
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

                }
                else
                {
                    this.Message = "Registration failed";
                }
            }
            else { 
              this.Message = "Registration failed";
                this.ViewModel.CompanyPhone = registration.CompanyPhone;
                this.ViewModel.CompanyName = registration.CompanyName;
                this.ViewModel.CompanyEmail = registration.CompanyEmail;
                this.ViewModel.Password = registration.Password;
                
            }

        }
    }
}
