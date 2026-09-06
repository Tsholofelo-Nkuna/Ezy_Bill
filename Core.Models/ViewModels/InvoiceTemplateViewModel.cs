using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Models.Base;

namespace ClientManagement.Models.ViewModels
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
