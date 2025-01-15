using Core.Presentation.ViewComponents.Interfaces.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Razor.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents
{
    public class PageBase<TModel> : RazorPage<TModel>, IPageBase<TModel>
    {
        public override Task ExecuteAsync()
        {
            return Task.CompletedTask;
        }

        public  Task ExecuteResultAsync(ActionContext context)
        {
            return Task.CompletedTask;
        }

        public virtual void OnInit(TModel viewModel)
        {}
    }
}
