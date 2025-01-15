using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Interfaces.Base
{
    public interface IPageBase<TModel> : IRazorPage, IActionResult
    {
        public void OnInit(TModel viewModel);
    }
}
