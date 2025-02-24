using Core.Presentation.ViewComponents.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MimeKit;
using System.Net.Mime;
using System.Text;

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController : Controllers.Base.ApiBaseController<AccountsController>
    {
        private UserManager<IdentityUser> _userManager { get; set; }
        public AccountsController(ILogger<AccountsController> logger, UserManager<IdentityUser> userManager) : base(logger)
        {
            _userManager = userManager;
        }

        // GET: AccountsController
        [HttpGet("[action]")]
        public async Task<string> ConfirmEmail(string userId, string code, string changedEmail)
        {
            var token = Encoding.UTF8.GetString(Convert.FromBase64String(code));
            if(_userManager.Users.FirstOrDefault(x => x.UserName == changedEmail) is IdentityUser user)
            {
               var result =  await _userManager.ConfirmEmailAsync(user, token);

                return result.Errors.FirstOrDefault()?.Description ?? "Thank you for confirming your email!"; 
            }
            else
            {
                return "Email confirmation failed, Please register!";
            }
        }

    }
}
