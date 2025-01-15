using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class OverviewLabelComponent
    {
        [Parameter]
        public string Text {  get; set; } = string.Empty;
    }
}
