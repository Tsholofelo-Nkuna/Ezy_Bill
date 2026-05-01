using Core.Presentation.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Runtime.CompilerServices;
using System.Text;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; } = new ForgotPasswordViewModel();
        public string Message { get; set; } = string.Empty;
        public string MessageColor => this.Message.Contains("success", StringComparison.OrdinalIgnoreCase) ? "success" : "danger";
        private readonly UserManager<IdentityUser> _userManager;
        public ForgotPasswordModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet(string? email, string? code)
        {
            this.ForgotPasswordViewModel.Email = email ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(code) && !string.IsNullOrWhiteSpace(email))
            {
                HttpContext.Session.SetString("code", code);
                HttpContext.Session.SetString("email", email);
            }
        }

        public async Task OnPostAsync(ForgotPasswordViewModel forgotPasswordData, [FromQuery] string? code)
        {
            var gotCode = HttpContext.Session.TryGetValue("code", out var codeBytes);
            var codeStr = gotCode ? Encoding.UTF8.GetString(codeBytes!): string.Empty;
            var gotEmail =  HttpContext.Session.TryGetValue("email", out var emailBytes);
            var email = gotEmail ? Encoding.UTF8.GetString(emailBytes!) : string.Empty;
            if (ModelState.IsValid && codeStr is not null
                && code == codeStr && email is not null
                && email.Equals(forgotPasswordData.Email, StringComparison.OrdinalIgnoreCase)
                && _userManager.Users.FirstOrDefault(x => x.UserName == forgotPasswordData.Email) is IdentityUser user)
            {

                var results = await _userManager.ResetPasswordAsync(user, Encoding.UTF8.GetString(Convert.FromBase64String(code)), forgotPasswordData.Password);
                if (results.Succeeded)
                {
                    this.Message = "Password reset successful.";
                }
                else
                {
                    this.Message = "Password reset failed.";
                }
            }
            this.ForgotPasswordViewModel.Password = forgotPasswordData.Password;
            this.ForgotPasswordViewModel.Email = forgotPasswordData.Email;  
            this.ForgotPasswordViewModel.ConfirmPassword = forgotPasswordData.ConfirmPassword;
        }
    }
}
