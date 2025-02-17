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

        public void OnPost(ForgotPasswordViewModel forgotPasswordData)
        {
            if (ModelState.IsValid)
            {
                // Do something
            }
            this.ForgotPasswordViewModel.Password = forgotPasswordData.Password;
            this.ForgotPasswordViewModel.Email = forgotPasswordData.Email;  
            this.ForgotPasswordViewModel.ConfirmPassword = forgotPasswordData.ConfirmPassword;
        }
    }
}
