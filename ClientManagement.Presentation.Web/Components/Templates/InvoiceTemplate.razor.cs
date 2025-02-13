using ClientManagement.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Models;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using Core.Presentation.Models.DataTransferObjects;

namespace ClientManagement.Presentation.Web.Components.Templates
{
    public partial class InvoiceTemplate : GenericComponentBase<InvoiceTemplateViewModel, InvoiceDto>
    {
        [Parameter]
        public InvoiceDto? Invoice {
            get => this.ViewModel.ViewModelState.FirstOrDefault();
            set
            {
                this.ViewModel.ViewModelState = value is not null ? Enumerable.Empty<InvoiceDto>().Append(value) : Enumerable.Empty<InvoiceDto>();

            }
        }

        [Parameter]
        public UserProfileDto UserProfile { get; set; } = new();    

    }
}
