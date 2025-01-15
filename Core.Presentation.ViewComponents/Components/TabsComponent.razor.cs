using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Presentation.ViewComponents.Components
{
    public partial class TabsComponent<TRecord> : GenericComponentBase<TabsComponentViewModel<TRecord>, TRecord> where TRecord : BaseDto, new()
    {
        [Parameter]
        public string Tab {  get; set; } = string.Empty;
        [Parameter] public EventCallback<string> TabChanged { get; set; } = new EventCallback<string>();
        [Parameter]
        public RenderFragment<TabItemViewModel>? TabItem { get; set; }
        public async Task OnTabItemClick(TabItemViewModel tabItemModel)
        {
            this.ViewModel.TabItems = this.ViewModel.TabItems.Select(x =>
            {
                x.Active = tabItemModel.Id == x.Id;
                return x;
            });
            this.Tab = this.ViewModel.TabItems.FirstOrDefault(x => x.Active)?.Id ?? string.Empty;
            await this.TabChanged.InvokeAsync(this.Tab);
          
        }
      
    }
}
