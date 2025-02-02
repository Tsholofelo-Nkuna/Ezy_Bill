using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utils.State.Interfaces
{
    public interface IAppState
    {
         ClaimsPrincipal? CurrentUser { get; set; }
         bool UserIsAuthenticated =>  CurrentUser is { Identity.IsAuthenticated : true };
    }
}
