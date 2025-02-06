using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Presentation.Models;


namespace ClientManagement.Presentation.Web.Components.Pages.Products.State
{
    public class ProductState
    {
        public ProductDto? NewProductToCreate { get; set; }
        public bool CreateNewProductModelIsValid { get; set; }
        public List<InputFieldViewModel<ProductDto>> NewlyCreatedProductFieldState { get; set; } = new List<InputFieldViewModel<ProductDto>>();
    }
}
