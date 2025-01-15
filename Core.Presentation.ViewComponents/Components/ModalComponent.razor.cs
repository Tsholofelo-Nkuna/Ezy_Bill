using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects.Base;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
namespace Core.Presentation.ViewComponents.Components
{
    public partial class ModalComponent<TRecordType> : GenericComponentBase<ModalViewModel<TRecordType>, TRecordType> where TRecordType : BaseDto, new()
    {
      
        
        [Parameter] public RenderFragment? Body { get; set; }
        [Parameter] public RenderFragment? Footer { get; set; }

    }
}
