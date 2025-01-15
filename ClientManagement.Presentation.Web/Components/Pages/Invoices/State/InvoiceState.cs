
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;

namespace ClientManagement.Presentation.Web.Components.Pages.Invoices.State
{
    public class InvoiceState
    {
        public List<InputFieldViewModel<InvoiceDto>> NewInvoiceFormFieldsState { get; set; } = new List<InputFieldViewModel<InvoiceDto>>();
        public InvoiceDto? NewlyCreateInvoice { get; set; }
        public bool CreateNewInvoiceFormIsValid { get; set; }
    }
}
