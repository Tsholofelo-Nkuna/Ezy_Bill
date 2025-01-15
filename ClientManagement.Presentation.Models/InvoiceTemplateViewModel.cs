using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.Presentation.Models
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
