using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Models.Base;
using ClientManagement.Models.Validation;
using ClientManagement.Models.Validation.Base;



namespace ClientManagement.Models.ViewModels
{
    public class InvoicesViewModel : GenericListViewModel<InvoiceDto>
    {
        public InvoicesViewModel(): this(Enumerable.Empty<InvoiceDto>()) { }
        public InvoicesViewModel(IEnumerable<InvoiceDto> state) : base(state)
        {
        }

        public FormComponentViewModel<InvoiceDto> InvoiceSearchFormViewModel
        {
            get;
            set;
        } = new FormComponentViewModel<InvoiceDto>(new[] {new InvoiceDto()}, "Invoices-Search-Form")
        {
            Title = "Filter",
            ActionName = "Invoices",
            ControllerName = "Get",
            Fields = new List<InputFieldViewModel<InvoiceDto>>
            {
                 new InputFieldViewModel<InvoiceDto>(nameof(InvoiceDto.ClientName), "Client Name")
                {
                    ControlType = ControlType.Text,

                },
                 new InputFieldViewModel<InvoiceDto>(nameof(InvoiceDto.PrimaryContact), "Client Phone")
                {
                    ControlType = ControlType.Text,
                    
                },
                  new InputFieldViewModel<InvoiceDto>(nameof(InvoiceDto.PrimaryEmail), "Client Email"
                      )
                {
                    
                    ControlType = ControlType.Text,

                },
                //new InputFieldViewModel<InvoiceDto>(nameof(InvoiceDto.DueDate), "Due date")
                //{
                //    ControlType = ControlType.Date,
                //},
                 new InputFieldViewModel<InvoiceDto>(nameof(InvoiceDto.Unpaid), "Show Unpaid Only")
                {
                    ControlType = ControlType.Checkbox
                    
                }
            },
            SubmitButtonText = "Filter"
        };

        public TableComponentViewModel<InvoiceDto> InvoicesTableViewModel { get; set; }
        = new TableComponentViewModel<InvoiceDto>(Enumerable.Empty<InvoiceDto>())
        {
            ColumnConfigs = new List<TableComponentColumnConfig<InvoiceDto>>
            {
               // new TableComponentColumnConfig<InvoiceDto>{ Index = nameof(InvoiceDto.Id), Name = "Invoice Id"},
                new TableComponentColumnConfig<InvoiceDto>{ Index = nameof(InvoiceDto.ClientName), Name = "Client Name"},
                new TableComponentColumnConfig<InvoiceDto>{ Index = nameof(InvoiceDto.Contact), Name = "Client Contact"},
                new TableComponentColumnConfig<InvoiceDto> { Index = nameof(InvoiceDto.Amount), Name = "Amount"},
                new TableComponentColumnConfig<InvoiceDto> { Index = nameof(InvoiceDto.PaidAmount), Name = "Paid Amount" },
                new TableComponentColumnConfig<InvoiceDto> { Index = nameof(InvoiceDto.OutstandingAmount), Name = "Outstanding Amount" },
                new TableComponentColumnConfig<InvoiceDto> { Index = nameof(InvoiceDto.DueDate), Name = "Due Date" }
            },
            ShowCreateNewButton = true,
            ShowArchiveButton = false,
            ShowDeleteButton = true,
            ShowViewButton = true,
            DeleteAction = "Delete",
            DeleteController = "Invoices",
            ViewAction = "Details",
            ViewController ="Invoices",
            GetDataAction = "Get",
            GetDataController="Invoices"
            
        };

        public ModalViewModel<InvoiceDto> NewInvoiceModalModel { get; set; }
        = new ModalViewModel<InvoiceDto>(Enumerable.Empty<InvoiceDto>())
        {
            Title = "New Invoice",
        };

        public FormComponentViewModel<InvoiceDto> NewInvoiceFormModel { get; set; }
        = new FormComponentViewModel<InvoiceDto>(new[] { new InvoiceDto() }, "Create-New-Invoice-Form")
        {
            Fields = new List<InputFieldViewModel<InvoiceDto>>
            {
                new InputFieldViewModel<InvoiceDto>(
                    nameof(InvoiceDto.DueDate), 
                    "Due Date",
                    new ValidatorBase<InvoiceDto>(new []{ Validators.Required()}, nameof(InvoiceDto.DueDate)))
                {
                    ControlType = ControlType.Date,
                },
                 new InputFieldViewModel<InvoiceDto>(
                    nameof(InvoiceDto.ClientId),
                    "Client",
                    new ValidatorBase<InvoiceDto>(new []{ Validators.Required()}, nameof(InvoiceDto.ClientId)))
                {
                    ControlType = ControlType.Select,
                    
                },
                  new InputFieldViewModel<InvoiceDto>(
                    nameof(InvoiceDto.ProductIdentifiers),
                    "Products/Services",
                    new ValidatorBase<InvoiceDto>(new []{ Validators.Required()}, nameof(InvoiceDto.ClientId)))
                {
                    ControlType = ControlType.MultiSelect,
                }

            },
            ColClass = "col-12"
        };
    }
}
