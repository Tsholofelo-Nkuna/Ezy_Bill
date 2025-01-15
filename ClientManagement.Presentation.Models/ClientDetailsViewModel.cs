
using Core.Presentation.Models.Base;

using Core.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;

namespace ClientManagement.Presentation.Models
{
    public class ClientDetailsViewModel : GenericListViewModel<ClientDto>
    {
        private readonly ClientsViewModel _clientsViewModel = new ClientsViewModel();
        public ClientDetailsViewModel(): this(Enumerable.Empty<ClientDto>()) {  }
        public ClientDetailsViewModel(IEnumerable<ClientDto> client): base(client) {

            this.DetailsFormViewModel = new(Enumerable.Empty<ClientDto>().Append(new ClientDto()), "ClientDetailsForm");
            this.PrimaryContactPersonFormViewModel = new(Enumerable.Empty<ClientDto>().Append(new ClientDto()), "ClientDetailsContactForm");
            this.DetailsFormViewModel.CollapseFooter = false;
            this.PrimaryContactPersonFormViewModel.CollapseFooter = false;
            this.PrimaryContactPersonFormViewModel.IncludeFormStatePropsAsHidden = true;
            this.DetailsFormViewModel.IncludeFormStatePropsAsHidden = true;
            this.DetailsFormViewModel.Fields = _clientsViewModel.NewClientFormViewModel.Fields;
            this.PrimaryContactPersonFormViewModel.Fields = _clientsViewModel.PrimaryContactFormViewModel.Fields;
            this.DetailsFormViewModel.ColClass = "col-12";
        }

        public FormComponentViewModel<ClientDto> DetailsFormViewModel { get; set; }
        public FormComponentViewModel<ClientDto> PrimaryContactPersonFormViewModel { get; set; }
        public TabsComponentViewModel<ClientDto> ClientTabConfig {  get; set; } = new TabsComponentViewModel<ClientDto>()
        {
            TabItems = new[] { 
                new TabItemViewModel("Details", "client-details-tab", "Details") { Active = true},
                new TabItemViewModel("Invoices", "client-invoices", "Invoices")
            }
        };
        public bool ShowNewClientModal {  get; set; }
        public TableComponentViewModel<InvoiceDto> ClientInvoiceTableViewModel { get; set; } =
            new TableComponentViewModel<InvoiceDto>() {
                ColumnConfigs = new List<TableComponentColumnConfig<InvoiceDto>>
                {
                    new TableComponentColumnConfig<InvoiceDto>
                    {
                        Index = nameof(InvoiceDto.Amount),
                        Name = "Amount",
                    },
                     new TableComponentColumnConfig<InvoiceDto>
                    {
                        Index = nameof(InvoiceDto.OutstandingAmount),
                        Name = "Outstanding Amount"
                    },
                      new TableComponentColumnConfig<InvoiceDto>
                    {
                        Index = nameof(InvoiceDto.DueDate),
                        Name = "Due Date"
                    }
                },
                ShowDeleteButton = false,
                ShowArchiveButton = false,
                ViewAction = "Details",
                ViewController = "Invoices",
            };
    }
}
