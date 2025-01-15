using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components.Templates
{
    public partial class HtmlTemplate<TContext>
    {
        [Parameter]
        public TContext? TemplateContext { get; set; }
        [Parameter]
        public RenderFragment<TContext?>? ContentTemplate { get; set; }
    }
}
