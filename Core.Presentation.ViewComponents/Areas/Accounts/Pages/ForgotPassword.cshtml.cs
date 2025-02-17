using Core.Presentation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; } = new ForgotPasswordViewModel();
        public void OnGet()
        {
        }
    }
}
