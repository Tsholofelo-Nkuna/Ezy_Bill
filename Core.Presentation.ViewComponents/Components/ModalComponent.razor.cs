using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
        public EventCallback<bool> ShowChanged { get; set; }

        public async Task OnCloseModalIconClick(MouseEventArgs args)
        {
            Show = false;
            await ShowChanged.InvokeAsync(Show);
        }

    }
}
