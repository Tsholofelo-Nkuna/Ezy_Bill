using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects.login;
using ClientManagement.Utils;
using ClientManagement.Utils.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MimeKit.Cryptography;
using Org.BouncyCastle.Bcpg;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Json;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
       // [Inject] private IHttpClientFactory _httpClientFactory { get; set; }
        public LoginModel(IHttpClientFactory httpClientFactory, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager)
        {
            _httpClient = httpClientFactory.CreateClient("AppApi");
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public LoginViewModel CredentialsDto { get; set; } = new LoginViewModel();
        public void OnGet()
        {
        }

        public async Task OnPostAsync(LoginViewModel userCredentials) {
            if (ModelState.IsValid)
            {
                  var response =  await this._httpClient.PostAsJsonAsync("/login?useCookies=true&useSessionCookies=true", new Dictionary<string, string> {
                        { "email", userCredentials.Username },
                        { "password", userCredentials.Password }
                    });
                if(response is { IsSuccessStatusCode : true })
                {
                   var user =  _userManager.Users.FirstOrDefault(x => x.UserName == userCredentials.Username)!;
                   await _signInManager.SignInAsync(user, false);
                   HttpContext.Response.Redirect("/");
                }
            }
            this.CredentialsDto.Password = userCredentials.Password;
            this.CredentialsDto.Username = userCredentials.Username;
        }
    }
}
