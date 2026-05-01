
using Core.Presentation.Models.ViewModels;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Core.Presentation.ViewComponents.Components;

namespace ClientManagement.Presentation.Desktop.Components.Pages.Invoices
{
    public partial class Details : GenericComponentBase<InvoiceDetailsViewModel, InvoiceDto>
    {
        
        [Parameter]
        public Guid Id { get; set; }
       
    }
}
