using Core.Presentation.ViewComponents.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ClientManagement.Presentation.Web.Controllers
{
    public class AccountsController : Controller
    {
        // GET: AccountsController
        public IActionResult Login()
        {
          return View();    
        }

    }
}
