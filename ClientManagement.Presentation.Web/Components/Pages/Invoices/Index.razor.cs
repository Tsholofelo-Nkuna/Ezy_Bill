using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;

namespace ClientManagement.Presentation.Web.Components.Pages.Invoices
{
    public partial class Index : GenericComponentBase<InvoicesViewModel, InvoiceDto>
    {
      
        public bool NewInvoiceCreationInProgress { get; set; }
        protected override async Task OnInitializedAsync()
        {
             await base.OnInitializedAsync();
            this.BaseUrl = "api/invoices";
            await PopulateDropdowns();
         
           
        }
        public bool InvoiceTableIsLoading { get; set; } = false;
        public InvoiceDto SearchFormFilters =>
            this.ViewModel.InvoiceSearchFormViewModel.ViewModelState.FirstOrDefault() ?? new InvoiceDto();
        public TableComponent<InvoiceDto>? InvoiceTableComponent {  get; set; }
        public async Task<IEnumerable<InvoiceDto>> GetData(InvoiceDto filters)
        {
            InvoiceTableComponent.ViewModel.PageRequest.Filters = filters;
            var response = await InvoiceTableComponent.GetPageData(this.InvoiceTableComponent.ViewModel.PageRequest);
            return  response?.Items ?? Enumerable.Empty<InvoiceDto>();
        }

        public async Task PopulateDropdowns()
        {
            var clientDropdown = this.ViewModel.NewInvoiceFormModel
              .Fields.FirstOrDefault(x => x.ControlType == ControlType.Select
              && x.Name == nameof(InvoiceDto.ClientId));
            if (clientDropdown is not null)
            {
                clientDropdown.Options = await this.GetClientDropListData();
            }

            var productsDropdown = this.ViewModel.NewInvoiceFormModel.Fields
                .FirstOrDefault(x => x.Name == nameof(InvoiceDto.ProductIdentifiers)
                 && x.ControlType == ControlType.MultiSelect
                );
            if(productsDropdown is { ControlType : ControlType.MultiSelect })
            {
                var response = await this.AppApi.PostAsJsonAsync<PageRequestDto<ProductDto>>("api/products/get", new() { GetAllPages = true});
                if(response is { IsSuccessStatusCode: true} validResponse)
                {
                    productsDropdown.Options = (await validResponse.Content.ReadFromJsonAsync<PageResponseDto<ProductDto>>())
                        ?.Items?.Select(x => (new KeyValuePair<string, string>(x.Id.ToString(), $"{x.Name} | {x.Description}")))
                        ?? Enumerable.Empty<KeyValuePair<string, string>>();
                }
            }
        }

        public async Task OnInvoiceDeleted(bool isDeleted)
        {
            if (isDeleted) {
                await this.GetData(SearchFormFilters);
               
            }
           
        }

        public Task OnViewInvoice(Guid invoiceId)
        {
            var tableViewModel = this.ViewModel.InvoicesTableViewModel;
            this.NavManager.NavigateTo($"{tableViewModel.ViewController}/{tableViewModel.ViewAction}/{invoiceId}");
            return Task.CompletedTask;
        }

        public async Task OnInvoiceSearchClick(IEnumerable<InvoiceDto> invoices)
        {
            this.InvoiceTableComponent.ViewModel.PageRequest.PageIndex = 0;
            await this.GetData(SearchFormFilters);
           
        }
        public async Task<IEnumerable<KeyValuePair<string, string>>> GetClientDropListData()
        {
           var response = await this.AppApi.PostAsJsonAsync<PageRequestDto<ClientDto>>("api/clients/get", new() { GetAllPages = true} );
            if (response.IsSuccessStatusCode)
            {
                var dropListData = await response.Content.ReadFromJsonAsync<PageResponseDto<ClientDto>>();

                return dropListData?.Items
                    ?.Select(x => new KeyValuePair<string, string>(x.Id.ToString(), $"{x.CompanyName} | {x.TradingAs}"))
                    ?? Enumerable.Empty<KeyValuePair<string, string>>();
            }
            else
            {
                return Enumerable.Empty<KeyValuePair<string, string>>();
            }
        }

        public async Task OnSaveNewInvoice(IEnumerable<InvoiceDto> data) 
        {
            this.NewInvoiceCreationInProgress = true;
            StateHasChanged();
            var response = await this.AppApi.PostAsJsonAsync($"{this.BaseUrl}", data.FirstOrDefault());
            if (response is { IsSuccessStatusCode: true } && (await response.Content.ReadFromJsonAsync<bool>()))
            {
                this.ViewModel.NewInvoiceModalModel.Show = false;
                await this.GetData(this.SearchFormFilters);
            }
            else
            {
                //Some error occured during creation of a new invoice.
            }
            this.NewInvoiceCreationInProgress = false;
            StateHasChanged();
           // return Task.CompletedTask;
        }
    }
}
