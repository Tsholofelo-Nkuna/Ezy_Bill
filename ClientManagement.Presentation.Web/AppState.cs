using System.Security.Claims;

namespace ClientManagement.Presentation.Web
{
    public class AppState
    {
        public ClaimsPrincipal? CurrentUser { get; set; }
        public bool UserIsAuthenticated => CurrentUser is { Identity.IsAuthenticated: true };
    }
}
