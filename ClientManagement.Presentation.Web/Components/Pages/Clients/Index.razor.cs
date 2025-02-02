using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;

using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace ClientManagement.Presentation.Web.Components.Pages.Clients
{
    public partial class Index : GenericComponentBase<ClientsViewModel, ClientDto>
    {
       
        [SupplyParameterFromForm(FormName = "NewClientDetails")]
        public ClientDto? NewClientDetails { get; set; }
        public TableComponent<ClientDto> ClientsTable { get; set; }
        public bool ClientsTableIsLoading { get; set; }
        public bool NewClientCreationInProgress { get; set; }
        public TableComponent<ClientDto>? ClientsTableComponent { get; set; }
        protected override async Task OnInitializedAsync()
        {
          
            await base.OnInitializedAsync();
            this.BaseUrl = "api/clients";
          

        }

        public  async Task<bool> AddNewClient(ClientDto newClient)
        {
            var response = await this.AppApi.PostAsJsonAsync<ClientDto>(this.BaseUrl, newClient);
            return response.IsSuccessStatusCode && (await response.Content.ReadFromJsonAsync<bool>());
        }
        private async Task GetData(ClientDto filters)
        {
            await (ClientsTableComponent?.GetData(filters) ?? Task.FromResult(Enumerable.Empty<ClientDto>()));
        }

        public async Task OnNewClientFormSubmitted(ClientDto? details, ClientDto? primaryContact)
        {
            this.NewClientCreationInProgress = true;
            StateHasChanged();
            var addedClient = new ClientDto();
            if (details is ClientDto newC && primaryContact is ClientDto contactInfo)
            {
                newC.PrimaryContactName = contactInfo.PrimaryContactName;
                newC.PrimaryContactPhone = contactInfo.PrimaryContactPhone;
                newC.PrimaryContactEmail = contactInfo.PrimaryContactEmail;
                addedClient = newC;
              
            }
           
            var contactFormIsValid = this.ViewModel.PrimaryContactFormViewModel.Validate();
            var detailsFormIsValid = this.ViewModel.NewClientFormViewModel.Validate();
           
            if (contactFormIsValid && detailsFormIsValid)
            {
                var added = await AddNewClient(addedClient);
                if (added)
                {
                    await this.GetData(SearchFormFilters);
                    this.ViewModel.ModalViewModel.Show = false;
                }
                else
                {
                    //Display error message
                }
            }
            this.NewClientCreationInProgress = false;
            StateHasChanged();           
        }
   
        public async Task OnSubmitSearchFilters(IEnumerable<ClientDto> searchFilters)
        {
            await this.GetData(this.SearchFormFilters);
           
        }

        public Task OnCreateNewTableRecord()
        {
            this.ViewModel.ModalViewModel.Show = true;
            StateHasChanged();
            return Task.CompletedTask;
        }

        public  Task OnViewTableRecord(Guid recordId)
        {
            this.NavManager.NavigateTo($"{this.ViewModel.TableConfig.ViewController}/{this.ViewModel.TableConfig.ViewAction}/{recordId}?{nameof(ClientDto.Archived)}={this.SearchFormFilters.Archived}");
            return Task.CompletedTask;
        }

        public async Task OnArchiveTableRecord(bool archived)
        {
            if (archived) { 
               await this.GetData(SearchFormFilters);
            }

           
        }

        public async Task OnDeleteTableRecord(bool deleted)
        {
            if (deleted)
            {
                await this.GetData(this.SearchFormFilters);
            }
            else
            {
                await JS.InvokeVoidAsync("alert", "Please ensure that this client doesn't\nhave any invoices attached to them before deleting.");
            }
           
        }
        public ClientDto SearchFormFilters {
            get {
             return this.ViewModel.SearchFormComponentViewModel.ViewModelState?.FirstOrDefault() ?? new ClientDto();
            } 
           
        } 
    }
}
