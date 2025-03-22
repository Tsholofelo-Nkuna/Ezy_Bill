using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;

using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;


namespace ClientManagement.Presentation.Web.Components.Pages.Products
{
    public partial class Index : GenericComponentBase<ProductsViewModel, ProductDto>
    {
        public ProductDto SearchFormFilters => 
            this.ViewModel.ProductSearchViewModel.ViewModelState.FirstOrDefault() ?? new ProductDto();
      
        public bool NewProductCreationInProgress { get; set; }
        public int ProductsTableEditIndex { get; set; } = -1;
        public bool ProductsTableIsLoading { get; set; }
        public TableComponent<ProductDto>? ProductsTableComponent { get; set; }
        protected override async Task OnInitializedAsync()
        {
             await base.OnInitializedAsync();
             this.BaseUrl = "api/products";
        }

        public async Task OnSearchClick(IEnumerable<ProductDto> searchState)
        {

            await this.GetData(this.SearchFormFilters);
          
        }
        public async Task<IEnumerable<ProductDto>> GetData(ProductDto filters)
        {
           this.ProductsTableComponent.ViewModel.PageRequest.Filters = filters;
           var pageResponse = await this.ProductsTableComponent.GetPageData(this.ProductsTableComponent.ViewModel.PageRequest);
           return pageResponse?.Items ?? Enumerable.Empty<ProductDto>();
        }

        public async  Task OnSaveProductUpdates(EventState<ProductDto?> eventState)
        {
            if (eventState.Success)
            {
                this.ProductsTableEditIndex = -1;
                await this.GetData(SearchFormFilters);
                StateHasChanged();
            }
           
        }
        public Task OnCreateNewProduct()
        {
            this.ViewModel.NewProductModalViewModel.Show = true;
            StateHasChanged();
            return Task.CompletedTask;
        }

        public async Task OnSaveNewProduct(IEnumerable<ProductDto> products)
        {
            this.NewProductCreationInProgress = true;
            StateHasChanged();
            if (this.ViewModel.NewProductFormViewModel.Validate())
            {
                var response = await this.AppApi
                     .PostAsJsonAsync($"{this.BaseUrl}", products.FirstOrDefault());
                if (response.IsSuccessStatusCode && await response.Content.ReadFromJsonAsync<bool>())
                {
                    this.ViewModel.NewProductModalViewModel.Show = false;
                    await this.GetData(this.SearchFormFilters);
                  
                   //StateHasChanged();
                }
            }
            else
            {
               // this.StateManager.Set<bool>(nameof(ProductState.CreateNewProductModelIsValid), false);
            }
            this.NewProductCreationInProgress=false;
            StateHasChanged();
        }

        public async Task OnDeleteProduct(bool deleted)
        {
            if (deleted)
            {
                await this.GetData(this.SearchFormFilters);
               
            }
            
        }
    }
}
