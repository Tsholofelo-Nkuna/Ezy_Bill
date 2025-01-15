using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components.Templates
{
    public partial class LabelValueTemplate
    {
        [Parameter]
        public RenderFragment? Label {  get; set; }
        [Parameter]
        public RenderFragment? Value { get; set; }
    }
}
