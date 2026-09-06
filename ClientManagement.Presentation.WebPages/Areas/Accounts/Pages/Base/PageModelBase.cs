using ClientManagement.Models.Base;
using ClientManagement.Models.DataTransferObjects.Base;
using ClientManagement.Models.Interfaces.Base;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Areas.Accounts.Pages.Base
{
    public class PageModelBase<TViewModel>: PageModel where TViewModel : new()
    {
        public TViewModel ViewModel { get; set; } = new TViewModel();

    }
}
