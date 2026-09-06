using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class PageComponent : GenericComponentBase<PageComponentViewModel, BaseDto>
    {
        [Parameter]
        public RenderFragment? Body { get; set; }
        [Parameter] 
        public RenderFragment? TopControls { get; set; }
        [Parameter]
        public bool ShowBackButton {  get; set; }
    }
}
