using ClientManagement.Utils.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ClientManagement.Presentation.Web.Areas.Admin.Pages
{
    public class CreateModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _appSettings;
        public CreateModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration appSettings)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._appSettings = appSettings;
        }
        public async Task OnGetAsync()
        {
            if (!this._roleManager.Roles.Any(x => x.Name == RoleConstants.SuperAdmin))
            {
                var roleCreationResult = await this._roleManager.CreateAsync(new() { Name = RoleConstants.SuperAdmin });
            }

            if (!this._userManager.Users.Any(x => x.UserName == this._appSettings["AdminAccount:Username"]))
            {
                var superAdminCreationResult = await this._userManager.CreateAsync(new() { UserName = this._appSettings["AdminAccount:Username"] }, this._appSettings["AdminAccount:Password"]);
            }
        }
    }
}
