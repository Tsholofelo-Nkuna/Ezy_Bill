using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components.Templates
{
    public partial class ConditionalTemplate<TContext, TOtherContext>
    {
        [Parameter]
        public bool Condition { get; set; }
        [Parameter]
        public TContext? ContextIfTrue { get; set; }
        [Parameter]
        public TOtherContext? ContextIfFalse { get; set; }
        [Parameter]
        public RenderFragment<TContext?>? IfTrue { get; set; }
        [Parameter]
        public RenderFragment<TOtherContext?>? ElseIfFalse { get; set; }
    }
}
