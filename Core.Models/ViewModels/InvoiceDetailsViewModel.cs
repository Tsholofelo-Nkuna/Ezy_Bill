using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.Base;
using Core.Presentation.Models.Validation;
using Core.Presentation.Models.Validation.Base;


namespace Core.Presentation.Models.ViewModels
{
    public class InvoiceDetailsViewModel : GenericListViewModel<InvoiceDto>
    {
        public InvoiceDetailsViewModel(): this(Enumerable.Empty<InvoiceDto>()) { }
        
        public InvoiceDetailsViewModel(IEnumerable<InvoiceDto> state) : base(state)
        {
        }
        public OverviewComponentViewModel<InvoiceDto> OverviewModel { get; set; } = new OverviewComponentViewModel<InvoiceDto>()
        {
            OverviewSectionsViewModel = new List<OverviewSectionViewModel> {
              new OverviewSectionViewModel {Title = "Invoice Details", Name = "InvoiceDetails"},
              new OverviewSectionViewModel{ Title = "Client Details", Name = "ClientDetails"},
              new OverviewSectionViewModel {Title = "Client Contacts", Name = "ClientContacts"}
             
            }
        };

        public TabsComponentViewModel<InvoiceDto> TabsViewModel { get; set; } = new TabsComponentViewModel<InvoiceDto>()
        {
            TabItems = new List<TabItemViewModel> { 
                new TabItemViewModel("Products/Services","invoice-products-tab","Products"){ Active = true },
                new TabItemViewModel("Payments","invoice-payments-tab","Payments")
            }
        };
        public TableComponentViewModel<InvoiceProductDto> InvoiceProductsTableViewModel { get; set; }
        = new TableComponentViewModel<InvoiceProductDto>
        {
            ShowArchiveButton = false,
            ShowViewButton = false,
            ShowDeleteButton = true,
            ShowCreateNewButton = true,
            ColumnConfigs = new List<TableComponentColumnConfig<InvoiceProductDto>>
            {
                new TableComponentColumnConfig<InvoiceProductDto>
                {
                    Index = nameof(InvoiceProductDto.ProductName),
                    Name = "Name"
                },
                 new TableComponentColumnConfig<InvoiceProductDto>
                {
                    Index = nameof(InvoiceProductDto.ProductDescription),
                    Name = "Description"
                },
                   new TableComponentColumnConfig<InvoiceProductDto>
                {
                    Index = nameof(InvoiceProductDto.ProductAmount),
                    Name = "Price Per Item"
                },
                   new TableComponentColumnConfig<InvoiceProductDto>
                {
                    Index = nameof(InvoiceProductDto.Quantity),
                    Name = "Quantity",
                    EditInputFieldViewModel = new InputFieldViewModel<InvoiceProductDto>(
                        nameof(InvoiceProductDto.Quantity),
                        "", 
                        new ValidatorBase<InvoiceProductDto>(new [] {Validators.Number() }, nameof(InvoiceProductDto.Quantity))
                        )
                   
                },
                     new TableComponentColumnConfig<InvoiceProductDto>
                {
                    Index = nameof(InvoiceProductDto.TotalCost),
                    Name = "Total Cost"
                },

            },
            DeleteAction = "Delete",
            DeleteController = "InvoiceProducts",
            GetDataAction = "Get",
            GetDataController = "InvoiceProducts"

            
        };
        public TableComponentViewModel<InvoicePaymentDto> InvoicePaymentsTableViewModel { get; set; }
        = new TableComponentViewModel<InvoicePaymentDto>
        {
            ColumnConfigs = new List<TableComponentColumnConfig<InvoicePaymentDto>>
            {
                new TableComponentColumnConfig<InvoicePaymentDto>
                {
                    Index = nameof(InvoicePaymentDto.PaymentDate),
                    Name = "Payment Date",
                    EditInputFieldViewModel = new InputFieldViewModel<InvoicePaymentDto>(
                         nameof(InvoicePaymentDto.PaymentDate),
                         "")
                    {
                        ControlType = ControlType.Date,
                    }
                        
                },
                 new TableComponentColumnConfig<InvoicePaymentDto>
                {
                    Index = nameof(InvoicePaymentDto.Amount),
                    Name = "Paid Amount",
                    EditInputFieldViewModel = new InputFieldViewModel<InvoicePaymentDto>(
                         nameof(InvoicePaymentDto.Amount),
                         "",
                         new ValidatorBase<InvoicePaymentDto>(new []{Validators.Number()}, nameof(InvoicePaymentDto.Amount)))
                   
                },
                   new TableComponentColumnConfig<InvoicePaymentDto>
                {
                    Index = nameof(InvoicePaymentDto.Comment),
                    Name = "Comment",
                    EditInputFieldViewModel = new InputFieldViewModel<InvoicePaymentDto>(
                         nameof(InvoicePaymentDto.Comment),
                         ""
                       )
                    {
                        ControlType = ControlType.Text,
                    }
                }
            },
            SaveOrUpdateController = "InvoicePayments",
            DeleteAction = "Delete",
            DeleteController = "InvoicePayments",
            ShowCreateNewButton = true,
            ShowDeleteButton = true,
            ShowArchiveButton = false,
            ShowViewButton = false,
            GetDataController= "InvoicePayments",
            GetDataAction="Get"
        };

        public ModalViewModel<InvoicePaymentDto> InvoicePaymentModalViewModel { get; set; } = new ModalViewModel<InvoicePaymentDto>
        {
            Title = "New Invoice Payment",
        };

        public ModalViewModel<InvoiceProductDto> InvoiceProductViewModalModel { get; set; } = new ModalViewModel<InvoiceProductDto>()
        {
            Title = "New Invoice Product/Service"
        };

        public FormComponentViewModel<InvoiceProductDto> NewInvoiceProductFormViewModel { get; set; } = new()
        {
            ViewModelState = Enumerable.Empty<InvoiceProductDto>().Append(new InvoiceProductDto()),
            Fields = new()
            {
                new InputFieldViewModel<InvoiceProductDto>( nameof(InvoiceProductDto.ProductId), "Product/Service")
                {
                    ControlType = ControlType.Select,
                },
                  new InputFieldViewModel<InvoiceProductDto>( nameof(InvoiceProductDto.Quantity), "Quantity", new ValidatorBase<InvoiceProductDto>(new []{ Validators.Number()}, nameof(InvoiceProductDto.Quantity)))
                {
                   
                },

            },
            ColClass = "col-12"
        };

        public FormComponentViewModel<InvoicePaymentDto> NewInvoicePaymentFormViewModel =
            new FormComponentViewModel<InvoicePaymentDto>(Enumerable.Empty<InvoicePaymentDto>().Append(new InvoicePaymentDto()), "NewInvoicePaymentForm")
            {
                Fields = new List<InputFieldViewModel<InvoicePaymentDto>>
                {
                     new InputFieldViewModel<InvoicePaymentDto>(
                        nameof(InvoicePaymentDto.PaymentDate),
                        "Payment Date",
                         new ValidatorBase<InvoicePaymentDto>(new [] {Validators.Required()}, nameof(InvoicePaymentDto.PaymentDate))
                        )
                     {
                         ControlType = ControlType.Date,
                     },
                    new InputFieldViewModel<InvoicePaymentDto>(
                        nameof(InvoicePaymentDto.Amount),
                        "Payment Amount",
                        new ValidatorBase<InvoicePaymentDto>(new [] {Validators.Number(), Validators.Required()}, nameof(InvoicePaymentDto.Amount))
                        ),
                    new InputFieldViewModel<InvoicePaymentDto>(
                        nameof(InvoicePaymentDto.Comment),
                        "Comment"

                        )
                    {
                        ControlType = ControlType.Text,
                    },
                },
                ColClass = "col-12"
            };
    }
}
