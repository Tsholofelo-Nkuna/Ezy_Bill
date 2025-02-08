using Core.Utils.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.ActionFilters
{
    public class ApiKeyActionFilter : IAsyncAuthorizationFilter
    {
        private readonly UserManager<IdentityUser> _userManager;
        public ApiKeyActionFilter(UserManager<IdentityUser> userManager) {
            _userManager = userManager;
        }
      

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue(AuthConstants.XApiKey, out var apiKey)
              && _userManager.Users.FirstOrDefault(x => x.UserName == apiKey.ToString()) is IdentityUser currentUser)
            {
                
            }
            else
            {
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }
}
