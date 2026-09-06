
using ClientManagement.Models.ViewModels;
using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Components.Templates;
using ClientManagement.Models;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Core.Presentation.ViewComponents.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Invoices
{
    public partial class Details : GenericComponentBase<InvoiceDetailsViewModel, InvoiceDto>
    {
        
        [Parameter]
        public Guid Id { get; set; }
       
    }
}
