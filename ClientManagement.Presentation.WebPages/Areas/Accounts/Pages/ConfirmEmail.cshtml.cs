using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc.RazorPages;

using System.Text;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        public string Message { get; set; } = string.Empty;
        public ConfirmEmailModel(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task OnGetAsync(string changedEmail, string code, string userId)
        {
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            if (_userManager.Users.FirstOrDefault(x => x.UserName == changedEmail) is IdentityUser user)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);

                this.Message =  result.Errors.FirstOrDefault()?.Description ?? "Thank you for confirming your email!";
            }
            else
            {
                this.Message = "Email confirmation failed, Please register!";
            }
        }
    }
}
