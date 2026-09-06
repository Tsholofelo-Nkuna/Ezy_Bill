using ClientManagement.Models;
using ClientManagement.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics.Contracts;
namespace Core.Presentation.ViewComponents.Components
{
    public partial class ModalComponent<TRecordType> : GenericComponentBase<ModalViewModel<TRecordType>, TRecordType> where TRecordType : BaseDto, new()
    {
      
        
        [Parameter] public RenderFragment? Body { get; set; }
        [Parameter] public RenderFragment? Footer { get; set; }

        [Parameter]
        public bool Show 
        {
            get => this.ViewModel.Show;
            set
            {
                this.ViewModel.Show = value; 
            }
        }

        [Parameter]
        public override IEnumerable<string> CssClassList { get; set; } = ["col-lg-4", "position-absolute", "top-0"]; 

        [Parameter]
        public string Width { get; set; } = "auto";
        [Parameter]
        public string Height { get; set; } = "auto";

        [Parameter]
        public EventCallback<bool> ShowChanged { get; set; }

        public async Task OnCloseModalIconClick(MouseEventArgs args)
        {
            Show = false;
            await ShowChanged.InvokeAsync(Show);
        }

    }
}
