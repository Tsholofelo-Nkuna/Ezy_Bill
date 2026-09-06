
using ClientManagement.Models.ViewModels;
using ClientManagement.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Clients
{
    public partial class Details : GenericComponentBase<ClientDetailsViewModel, ClientDto>
    {
        [Parameter]
        public Guid Id { get; set; }
    }
}
