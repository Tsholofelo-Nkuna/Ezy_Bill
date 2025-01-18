using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components.Templates
{
    public partial class ButtonTemplate
    {
        [Parameter]
        public string CssClass { get; set; } = "btn btn-primary";
        [Parameter]
        public string Text { get; set; } = string.Empty;
        [Parameter]
        public RenderFragment? Icon { get; set; }
        [Parameter]
        public Func<Task>? OnClick { get; set; }
        [Parameter]
        public string? ToolTip { get; set; }
        [Parameter]
        public bool IsLoading {get; set;}
        public void OnClicked()
        {
            if (!IsLoading)
            {
                OnClick?.Invoke();
            }
        }
        
    }


}
