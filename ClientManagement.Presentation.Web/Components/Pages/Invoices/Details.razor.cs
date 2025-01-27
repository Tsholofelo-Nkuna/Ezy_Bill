
using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Components.Templates;
using Core.Presentation.Models;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using Microsoft.AspNetCore.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Invoices
{
    public partial class Details : GenericComponentBase<InvoiceDetailsViewModel, InvoiceDto>
    {
        [Parameter]
        public Guid Id { get; set; }
        public int InvoiceProductsTableEditIndex { get; set; } = -1;
        public int InvoicePaymentsTableEditIndex { get; set; } = -1;
        public InvoiceDto? Invoice => ViewModel.OverviewModel.ViewModelState.FirstOrDefault();
        private string _invoiceDetailsTab = "products";
        [Inject]
        public HtmlToPdfConverter? HtmlToPdfConverter { get; set; }
        public string InvoicePdfBase64 { get; set; } = string.Empty;
        public bool PrintBusy { get; set; }
        public bool InvoicePaymentInProgress { get; set; }
        public string InvoiceDetailsTab 
        {
            get => _invoiceDetailsTab;
            set
            {
                _invoiceDetailsTab = value;
                if(_invoiceDetailsTab.Equals("invoice-payments-tab", StringComparison.OrdinalIgnoreCase))
                {
                    this.GetInvoicePayments(this.Id);
                }
            }
        }
        public double InvoicedProductsTotalAmount {
            get
            {
                return this.ViewModel
                    .InvoiceProductsTableViewModel
                    .ViewModelState
                    .Sum(x => x.TotalCost);
              
            }
         }
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            this.BaseUrl = "api/InvoicePayments";
            await this.GetData();
        }

        public async Task GetData(bool invokeStateHasChange = false)
        {
            var apiReaponse = await this.AppApi.PostAsJsonAsync($"api/Invoices/Get", new InvoiceDto { Id = this.Id});
            if (apiReaponse.IsSuccessStatusCode)
            {
              this.ViewModel.OverviewModel.ViewModelState = (await apiReaponse.Content.ReadFromJsonAsync<IEnumerable<InvoiceDto>>()) ?? Enumerable.Empty<InvoiceDto>(); 
              this.ViewModel.InvoiceProductsTableViewModel.ViewModelState =
              this.ViewModel.OverviewModel.ViewModelState.FirstOrDefault()?.InvoiceProducts ?? this.ViewModel.InvoiceProductsTableViewModel.ViewModelState;
                if (invokeStateHasChange)
                {
                    StateHasChanged();
                }
            }
        }

        public async Task GetInvoicePayments(Guid invoiceId)
        {
            var invoicePaymentFilter = new InvoicePaymentDto { InvoiceId = Id };
            var response = await this.AppApi.PostAsJsonAsync($"{this.BaseUrl}/Get", invoicePaymentFilter);
            if (response.IsSuccessStatusCode)
            {
                this.ViewModel.InvoicePaymentsTableViewModel.ViewModelState = (await response.Content.ReadFromJsonAsync<IEnumerable<InvoicePaymentDto>>( )) ??
                                                                                Enumerable.Empty<InvoicePaymentDto>();
                StateHasChanged();
            }
        }
        public async Task OnPrintInvoice()
        { 
            PrintBusy = true;
            StateHasChanged();
            var invoiceTemplate = new InvoiceTemplate();
            var pdfContent =  await (this.HtmlToPdfConverter?.CreatePdfAsync(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Now:yyyyMMddhhmmss}-{this.Id}.pdf"),
                invoiceTemplate,
                ParameterView.FromDictionary(new Dictionary<string, object?> { { nameof(InvoiceTemplate.Invoice), this.Invoice } })) ?? Task.FromResult(new byte[] { }));
            if(pdfContent is byte[] contents)
            {
                this.InvoicePdfBase64 = $"data:application/pdf;base64,{Convert.ToBase64String(pdfContent)}";
               
            }
            PrintBusy = false;
            StateHasChanged();
        }
        public async Task OnEditInvoicePaymentSaveClick(EventState<InvoicePaymentDto?> eventState)
        {
            if (eventState.Success)
            {
                this.InvoicePaymentsTableEditIndex = -1;
                await this.GetData(true);
                await this.GetInvoicePayments(this.Id);
            }
        }
        public async Task OnNewInvoicePaymentSave(EventState<IEnumerable<InvoicePaymentDto>?> eventState)
        {
            this.InvoicePaymentInProgress = true;
            this.StateHasChanged();
            if(eventState is { Success : true, Payload : IEnumerable<InvoicePaymentDto>} && eventState.Payload.Any())
            {
                var saveUrl = $"{BaseUrl}";
                var savedPayment = eventState.Payload.FirstOrDefault();
               
                if(savedPayment is not null)
                {
                    savedPayment.InvoiceId = this.Id;
                }
                var saveResponse = await this.AppApi.PostAsJsonAsync(saveUrl, savedPayment);
                if (saveResponse.IsSuccessStatusCode)
                {
                    ViewModel.InvoicePaymentModalViewModel.Show = false;
                    await this.GetData(true);
                    await this.GetInvoicePayments(this.Id);
                }
            }

            this.InvoicePaymentInProgress = false;
            this.StateHasChanged();
           
        }
        public Task OnNewInvoicePaymentClicked(EventState<InvoicePaymentDto?> eventState)
        {
            if (eventState.Success)
            {
                this.ViewModel.InvoicePaymentModalViewModel.Show = true;
                StateHasChanged();
               
            }
           
            return Task.CompletedTask;
        }
        public InvoiceDto? OverviewModelState  => ViewModel.OverviewModel.ViewModelState.FirstOrDefault();
        
        public async Task OnDeleteInvoiceProduct(bool isDeleted)
        {
            if (isDeleted)
            {
                await this.GetData(true);
            }
        }

        public async Task OnDeleteInvoicePayment(EventState<InvoicePaymentDto?> eventStatus)
        {
            if (eventStatus.Success) {
                await this.GetData(true);
                await this.GetInvoicePayments(this.Id);
            }
           
        }
        public async Task OnSaveInvoiceProduct(EventState<InvoiceProductDto?> eventState)
        {
            if (!eventState.Success && eventState.Payload is not null)
            {
              var apiResponse = await this.AppApi.GetAsync($"api/invoiceProducts/UpdateInvoiceProductQuantity/{eventState.Payload.Id}?quantity={eventState.Payload.Quantity}");
              if(apiResponse.IsSuccessStatusCode && await apiResponse.Content.ReadFromJsonAsync<bool>())
                {
                    this.InvoiceProductsTableEditIndex = -1;
                    await this.GetData(true);
                }
                else
                {
                    //Display error report to user
                }
            }
            else
            {
                //Display error report to user
            }
          
        }
    }
}
