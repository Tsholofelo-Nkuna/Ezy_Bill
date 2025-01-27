
using Core.Presentation.Models.Base;
using Core.Presentation.Models.Validation.Base;
using Core.Presentation.Models.Validation;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;



namespace ClientManagement.Presentation.Models
{
    public class ClientsViewModel : GenericListViewModel<ClientDto>
    {
        public ClientsViewModel() : this(Enumerable.Empty<ClientDto>()){ }
        public ClientsViewModel(IEnumerable<ClientDto> clients): base(clients)
        {
           
        }
        public FormComponentViewModel<ClientDto> SearchFormComponentViewModel { get; set; } = new FormComponentViewModel<ClientDto>(Enumerable.Empty<ClientDto>().Append(new ClientDto()), "ClientSearchForm")
        {
            ColClass = "col-4",
            SubmitButtonText = "Filter",
            Fields = new(){

                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.CompanyName),
                    "Company Name"
                    ),

                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.Archived),
                    "Show Archived Only"
                    
                    )
                {
                    ControlType = ControlType.Checkbox,
                }
            },
            ActionName = string.Empty, //nameof(HomeController.Index),
            ControllerName = "Home",
            FormName = "ClientSearchForm"
        };
       
        public TableComponentViewModel<ClientDto> TableConfig { get; set; } = new TableComponentViewModel<ClientDto>(new List<ClientDto>()){
            ColumnConfigs = new List<TableComponentColumnConfig<ClientDto>>()
            {
                new TableComponentColumnConfig<ClientDto>() { Index = nameof(ClientDto.CompanyName), Name = "Company name"},
                new TableComponentColumnConfig<ClientDto>() { Index = nameof(ClientDto.TradingAs), Name = "Trading as"},
                new TableComponentColumnConfig<ClientDto>() { Index = nameof(ClientDto.LandlineNumber), Name = "Landline no."},
                new TableComponentColumnConfig<ClientDto>() { Index = nameof(ClientDto.Province), Name = "Province"},
                new TableComponentColumnConfig<ClientDto>() { Index = nameof(ClientDto.Address), Name = "Address"}
            },
            DeleteAction = "Delete",//nameof(HomeController.Delete),
            DeleteController = "Clients",
            ArchiveAction = "Archive", //nameof(HomeController.Archive),
            ArchiveController = "Clients",
            ViewAction = "Details",//nameof(HomeController.Details),
            ViewController = "Clients",
            ShowCreateNewButton = true,
            GetDataController = "Clients",
            GetDataAction = "Get"
        };

        public FormComponentViewModel<ClientDto> NewClientFormViewModel { get; set; } = new(Enumerable.Empty<ClientDto>().Append(new ClientDto()), "NewClientForm")
        {
            FormName = "NewClientDetails",
            Fields = new()
            {

                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.CompanyName), 
                    "Name / Company Name",
                    new ValidatorBase<ClientDto>(new ValidatorFn[] { Validators.Required()}, nameof(ClientDto.CompanyName))
                    ),
                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.TradingAs),
                    "Surname / Trading As",
                    new ValidatorBase<ClientDto>(new ValidatorFn[] { Validators.Required()}, nameof(ClientDto.TradingAs))
                    ),
                 new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.LandlineNumber),
                    "Landline No",
                    new ValidatorBase<ClientDto>(new ValidatorFn[] { Validators.Required()}, nameof(ClientDto.LandlineNumber))
                    ),
                 new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.Province),
                    "Province",
                    new ValidatorBase<ClientDto>(new ValidatorFn[] { Validators.Required()}, nameof(ClientDto.Province))
                    ),
                 new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.Address),
                    "Address",
                    new ValidatorBase<ClientDto>(new ValidatorFn[] { Validators.Required()}, nameof(ClientDto.Address))
                    ),
            },
            ColClass = "col-12",
            ActionName = "/",//nameof(HomeController.AddOrUpdate),
            ControllerName = "Home",
            CollapseFooter = true,
        };
        public FormComponentViewModel<ClientDto> PrimaryContactFormViewModel { get; set; } = new (Enumerable.Empty<ClientDto>().Append(new ClientDto()), "ClientContactForm")
        {
            Fields = new()
            {

                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.PrimaryContactName),
                    "Name",
                    new ValidatorBase<ClientDto>(new [] { Validators.Required()}, nameof(ClientDto.PrimaryContactName))
                    ) { 
                  ControlType = ControlType.Text
                },
                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.PrimaryContactEmail),
                    "Email",
                    new ValidatorBase<ClientDto>(new [] { Validators.Required()}, nameof(ClientDto.PrimaryContactEmail))
                    ) {
                  ControlType = ControlType.Email,
                },
                new InputFieldViewModel<ClientDto>(
                    nameof(ClientDto.PrimaryContactPhone),
                    "Contact",
                    new ValidatorBase<ClientDto>(new [] { Validators.Required()}, nameof(ClientDto.PrimaryContactPhone))
                    ){ 
                  ControlType = ControlType.Text,
                },
            },
            ColClass = "col-12",
            ActionName = string.Empty, //nameof(HomeController.AddOrUpdate),
            ControllerName = "Home",
            CollapseFooter = true,
            FormName = "NewClientPrimaryContactForm"
            
        };
        public ModalViewModel<ClientDto> ModalViewModel { get; set; } = new ModalViewModel<ClientDto>
        {
            Title = "New Client"
        };
   
      
    }
}
