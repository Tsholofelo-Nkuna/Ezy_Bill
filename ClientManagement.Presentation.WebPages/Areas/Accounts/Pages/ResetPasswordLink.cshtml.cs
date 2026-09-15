using ClientManagement.Utils.Constants;
using ClientManagement.Utils.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class ResetPasswordLinkModel : PageModel
    {
        private readonly MailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        [Required, EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ResetPasswordLinkModel(MailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _emailSender = emailSender;
            _userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task OnPostAsync(string Email)
        {
            if (ModelState.IsValid && ( await _userManager.FindByEmailAsync(Email)) is IdentityUser user)
            {
               var token = await _userManager.GeneratePasswordResetTokenAsync(user);
               var code = Convert.ToBase64String(Encoding.UTF8.GetBytes(token));
               var queryStr = QueryString.Create(new Dictionary<string, string>
               {
                   { "email", Email },
                   { "code", code }
               });
                await _emailSender.SendPasswordResetLinkAsync(user.UserName!, Email, $"{Request.Scheme}://{Request.Host}{LoginPathConstants.ForgotPassword}{queryStr.Value}");
                this.Message = "Password reset link sent to your email. Please check your email.";
            }
           
        }
    }
}
