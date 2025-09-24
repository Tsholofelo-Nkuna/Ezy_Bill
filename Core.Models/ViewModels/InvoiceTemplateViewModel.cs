using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.Base;

namespace Core.Presentation.Models.ViewModels
{
    public class InvoiceTemplateViewModel : GenericListViewModel<InvoiceDto>
    {
        public InvoiceTemplateViewModel() : base(Enumerable.Empty<InvoiceDto>())
        {

        }
        public InvoiceTemplateViewModel(IEnumerable<InvoiceDto> state) : base(state)
        {
          
        }
    }
}
