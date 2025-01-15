using ClientManagement.Presentation.Models;
using ClientManagement.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Components.Pages.Products.State;
using Core.Presentation.Models;

using Core.Presentation.ViewComponents.Components;
using Core.Presentation.ViewComponents.Components.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Extensions;
using System.Globalization;

namespace ClientManagement.Presentation.Web.Components.Pages.Products
{
    public partial class Index : GenericComponentBase<ProductsViewModel, ProductDto>
    {
        public ProductDto SearchFormFilters => 
            this.ViewModel.ProductSearchViewModel.ViewModelState.FirstOrDefault() ?? new ProductDto();
        [Inject]
        public ProductStateManager StateManager { get; set; }
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
           return await (this.ProductsTableComponent?.GetData(filters) ?? Task.FromResult(Enumerable.Empty<ProductDto>()) );
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
