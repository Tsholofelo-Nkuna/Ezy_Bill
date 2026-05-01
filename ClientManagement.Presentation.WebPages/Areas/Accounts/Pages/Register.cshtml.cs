using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Areas.Accounts.Pages.Base;
using Core.Utils.Constants;
using Core.Utils.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Globalization;
using System.Net.Http.Json;
using System.Text;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class RegisterModel : PageModelBase<RegisterViewModel>
    {
        private readonly HttpClient _httpClient;
        public string Message { get; set; } = string.Empty;
        
        public string MessageColor => this.Message.Contains("success", StringComparison.OrdinalIgnoreCase) ? "success" : "danger";
        private readonly UserManager<IdentityUser> _userManager;
        private readonly MailSender _emailSender;
        public RegisterModel(IHttpClientFactory httpClientFactory,
            UserManager<IdentityUser> userManager,
            MailSender emailSender) { 
           this._httpClient = httpClientFactory.CreateClient("AppApi");
            this._userManager = userManager;
            _emailSender = emailSender;
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
                    this.Message = "Registration successful.";
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
                        this.Message = validResponse.Message+ ", email verification required. A verification link has been sent to your email.";
                        var token =  await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var tokenBytes = Encoding.UTF8.GetBytes(token);
                        var qBuilder = QueryString.Create(new Dictionary<string, string>
                        {
                            { "changedEmail", newUser.Email },
                            { "userid", newUser.Id },
                            { "code", Convert.ToBase64String(tokenBytes)  }
                        });
                       
                        await _emailSender.SendConfirmationLinkAsync(newUser.UserName!, newUser.Email, $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host}/Accounts/confirmemail{qBuilder.Value}");
                        //var confirmEmail = await this._httpClient.GetAsync($"/confirmemail?{qBuilder.Value}");
                        //var text = await confirmEmail.Content.ReadAsStringAsync();
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
