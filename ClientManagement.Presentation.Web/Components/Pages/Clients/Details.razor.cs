
using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models.DataTransferObjects;
using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;


namespace ClientManagement.Presentation.Web.Components.Pages.Clients
{
    public partial class Details : GenericComponentBase<ClientDetailsViewModel, ClientDto>
    {
        [Parameter]
         public Guid Id { get; set; }
        [SupplyParameterFromQuery]
        public bool Archived { get; set; }
      
        private string _detailsTabId = string.Empty;
        public string DetailsTabId
        { 
            get => _detailsTabId; 
            set
            {
                _detailsTabId = value;
                if(_detailsTabId.Equals("client-invoices", StringComparison.OrdinalIgnoreCase))
                {
                    this.GetClientInvoices(true);
                }
            } 
        }

        public ClientDto DetailsFilter => this.ViewModel.ViewModelState.FirstOrDefault() ?? new ClientDto();
        public TableComponent<InvoiceDto>? ClientInvoiceTable {  get; set; }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.BaseUrl = "api/clients";
            await this.GetData(new ClientDto { Id = Id, Archived = this.Archived });
           
        }

        public Task OnViewClientInvoiceClicked(Guid invoiceId)
        {
            var navUrl = $"{this.ViewModel.ClientInvoiceTableViewModel.ViewController}/{this.ViewModel.ClientInvoiceTableViewModel.ViewAction}/{invoiceId}";
            this.NavManager.NavigateTo(navUrl);
            return Task.CompletedTask;
        }

        public async Task GetData(ClientDto filter)
        {
            var response = await this.AppApi.GetAsync($"{this.BaseUrl}/GetByArchive/{filter.Id}?archived={filter.Archived}");
            if (response.IsSuccessStatusCode) { 
              var result  = await response.Content.ReadFromJsonAsync<ClientDto?>();
              this.ViewModel.ViewModelState = new[] { result ?? new ClientDto() };
              this.ViewModel.PrimaryContactPersonFormViewModel.ViewModelState = this.ViewModel.ViewModelState;
              this.ViewModel.DetailsFormViewModel.ViewModelState = this.ViewModel.ViewModelState;
              StateHasChanged();
            }
        }

        public async Task GetClientInvoices(bool invokeStateHasChanged = false)
        {
         
            var apiResponse = await this.ClientInvoiceTable.GetPageData(new() { Filters = new InvoiceDto { ClientId = this.Id.ToString() } });
            if (apiResponse is PageResponseDto<InvoiceDto> validResponse) {
                //this.ViewModel.ClientInvoiceTableViewModel
                //   .ViewModelState = validResponse.Items;
                //if (invokeStateHasChanged)
                //{
                //    StateHasChanged();
                //}
            }
        }

        public async Task OnSaveClientDetails(IEnumerable<ClientDto> clientDetailUpdates)
        {
            if (clientDetailUpdates.Any()) {
                var detailUpdateReponse = await this.AppApi.PostAsJsonAsync<ClientDto>(this.BaseUrl, clientDetailUpdates.FirstOrDefault()!);
                if (detailUpdateReponse.IsSuccessStatusCode && (await detailUpdateReponse.Content.ReadFromJsonAsync<bool>()))
                {
                    await this.GetData(this.DetailsFilter);
                }
            }  
          
        }

        public async Task OnSaveClientContacts(IEnumerable<ClientDto> clientContactUpdates)
        {
            if (clientContactUpdates.Any())
            {
                var contactUpdateResponse = await this.AppApi.PostAsJsonAsync<ClientDto>(this.BaseUrl, clientContactUpdates.FirstOrDefault()!);
                if(contactUpdateResponse.IsSuccessStatusCode && await contactUpdateResponse.Content.ReadFromJsonAsync<bool>())
                {
                    await this.GetData(this.DetailsFilter);
                }
            }
         
           
        }
    }
}
