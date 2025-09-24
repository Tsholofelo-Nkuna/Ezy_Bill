
using Core.Presentation.Models.ViewModels;
using Core.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Components.Templates;
using Core.Presentation.Models;
using Core.Presentation.ViewComponents.Components.Base;
using Core.Presentation.ViewComponents.Utils.DocumentGeneration.Pdf;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Core.Presentation.ViewComponents.Components;

namespace ClientManagement.Presentation.Web.Components.Pages.Invoices
{
    public partial class Details : GenericComponentBase<InvoiceDetailsViewModel, InvoiceDto>
    {
        [Inject] private UserManager<IdentityUser>? _userManager { get; set; }
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
        public bool InvoiceProductCreationInProgress { get; set; }
        public TableComponent<InvoicePaymentDto>? InvoicePaymentTable { get; set; }
        public TableComponent<InvoiceProductDto>? InvoiceProductTable { get; set; }
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
                else if(_invoiceDetailsTab.Equals("invoice-products-tab", StringComparison.OrdinalIgnoreCase))
                {
                    this.GetInvoiceProducts(this.Id);
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
            await this.GetInvoice();
            await this.GetInvoiceProducts(this.Id);
        }
        public async Task GetInvoice(bool invokeStateHasChange = false)
        {
            var apiReaponse = await this.AppApi.PostAsJsonAsync<PageRequestDto<InvoiceDto>>($"api/Invoices/Get", new() { Filters = new() { Id = this.Id }, GetAllPages = true });
            
            if (apiReaponse.IsSuccessStatusCode)
            {
              this.ViewModel.OverviewModel.ViewModelState = (await apiReaponse.Content.ReadFromJsonAsync<PageResponseDto<InvoiceDto>>())?.Items ?? []; 
            //  this.ViewModel.InvoiceProductsTableViewModel.ViewModelState =
             // this.ViewModel.OverviewModel.ViewModelState.FirstOrDefault()?.InvoiceProducts ?? this.ViewModel.InvoiceProductsTableViewModel.ViewModelState;
                if (invokeStateHasChange)
                {
                    StateHasChanged();
                }
            }

            var productRequestResponse = await this.AppApi.PostAsJsonAsync<PageRequestDto<ProductDto>>($"api/Products/Get", new() { GetAllPages = true});
            if(productRequestResponse is { IsSuccessStatusCode : true} successResponse)
            {
                var productList = (await successResponse.Content.ReadFromJsonAsync<PageResponseDto<ProductDto>>())?.Items ?? [] ;
                var productOptions = productList?
                    .Select(x => new KeyValuePair<string, string>(x.Id.ToString(), $"{x.Name} | {x.Description}"));
                if(productOptions is IEnumerable<KeyValuePair<string, string>> validProductOptions)
                {
                   var productDropDown = this.ViewModel.NewInvoiceProductFormViewModel.Fields
                        .FirstOrDefault(x => x.ControlType == ControlType.Select && x.Name == nameof(InvoiceProductDto.ProductId));
                    if(productDropDown is not null)
                    {
                        productDropDown.Options = validProductOptions;
                        StateHasChanged() ;
                    }
                }
            }
        }
        public async Task GetInvoicePayments(Guid invoiceId)
        {
            var invoicePaymentRequest = new PageRequestDto<InvoicePaymentDto> { Filters = new() { InvoiceId = this.Id }, GetAllPages = true };
            
            var response =  (await this.InvoicePaymentTable.GetPageData(invoicePaymentRequest));
            //if (response is PageResponseDto<InvoicePaymentDto> validResponse)
            //{
            //    this.ViewModel.InvoicePaymentsTableViewModel.ViewModelState = validResponse.Items;
            //    StateHasChanged();
            //}
        }
        public async Task GetInvoiceProducts(Guid invoiceId)
        {
            var invoicePaymentRequest = new PageRequestDto<InvoiceProductDto> { Filters = new() { InvoiceId = this.Id }, GetAllPages = true };

            var response = (await this.InvoiceProductTable.GetPageData(invoicePaymentRequest));
        }
        public async Task OnPrintInvoice()
        { 
            PrintBusy = true;
            StateHasChanged();
            var invoiceTemplate = new InvoiceTemplate();
           
            var currentUserData = (_userManager.Users.FirstOrDefault(x => x.UserName == this.CurrentUser.Identity.Name));
            var userProfileResponse = await this.AppApi.GetFromJsonAsync<ResponseDto<UserProfileDto>> ($"api/UserProfiles/{currentUserData.Id}");
            if(userProfileResponse is { Data: UserProfileDto} successUserProfileResponse)
            {
               var pdfContent = await (this.HtmlToPdfConverter?.CreatePdfAsync(
               Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{DateTime.Now:yyyyMMddhhmmss}-{this.Id}.pdf"),
               invoiceTemplate,
               ParameterView.FromDictionary(new Dictionary<string, object?> {
                    { nameof(InvoiceTemplate.Invoice), this.Invoice },
                    { nameof(InvoiceTemplate.UserProfile), successUserProfileResponse.Data }
               })) ?? Task.FromResult(new byte[] { }));
                if (pdfContent is byte[] contents)
                {
                    this.InvoicePdfBase64 = $"data:application/pdf;base64,{Convert.ToBase64String(pdfContent)}";

                }
            }
           
            PrintBusy = false;
            StateHasChanged();
        }
        public async Task OnEditInvoicePaymentSaveClick(EventState<InvoicePaymentDto?> eventState)
        {
            if (eventState.Success)
            {
                this.InvoicePaymentsTableEditIndex = -1;
                await this.GetInvoice(true);
                await this.GetInvoicePayments(this.Id);
            }
        }
        public async Task OnNewInvoiceProductSave (EventState<IEnumerable<InvoiceProductDto?>> eventState)
        {
            this.InvoiceProductCreationInProgress = true;
            this.StateHasChanged();
            if(eventState is { Success : true, Payload : IEnumerable<InvoiceProductDto> } && eventState.Payload.Any(iP =>  iP is not null && iP.ProductId != Guid.Empty))
            {
                var invoiceProduct = eventState.Payload.FirstOrDefault(ip => ip is not null && ip.ProductId != Guid.Empty)!;
                invoiceProduct.InvoiceId = this.Id;
                
                var serviceReponse = await this.AppApi.PostAsJsonAsync("api/InvoiceProducts", invoiceProduct);
                if(serviceReponse is { IsSuccessStatusCode : true } successServiceResponse && (await successServiceResponse.Content.ReadFromJsonAsync<bool>()))
                {
                    await this.GetInvoice(true);
                    await this.GetInvoiceProducts(this.Id);
                }
                
            }
            this.ViewModel.InvoiceProductViewModalModel.Show = false;
            this.InvoiceProductCreationInProgress = false;
            this.StateHasChanged();
        }
        public Task OnInvoiceProductTableNewClick()
        {
            this.ViewModel.InvoiceProductViewModalModel.Show = true;
            StateHasChanged();
            return Task.CompletedTask;
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
                    await this.GetInvoice(true);
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
                await this.GetInvoice(true);
                await this.GetInvoiceProducts(this.Id);
                StateHasChanged();
            }
        }
        public async Task OnDeleteInvoicePayment(EventState<InvoicePaymentDto?> eventStatus)
        {
            if (eventStatus.Success) {
                await this.GetInvoice(true);
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
                    await this.GetInvoice(true);
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
