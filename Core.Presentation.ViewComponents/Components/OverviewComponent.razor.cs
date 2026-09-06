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
    public partial class OverviewComponent<TRecord> : GenericComponentBase<OverviewComponentViewModel<TRecord>, TRecord> where TRecord: BaseDto, new() 
    {
        [Parameter]
        public RenderFragment<string>?  OverviewSection { get; set; }
    }
}
