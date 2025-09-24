using Core.Presentation.Models.DataTransferObjects.Base;
using System.ComponentModel.DataAnnotations;


namespace Core.Presentation.Models.DataTransferObjects
{
    public class InvoiceProductDto : BaseDto
    {
        public Guid ProductId { get; set; }
        public ProductDto Product { get; set; } = new ProductDto();
        public double Quantity { get; set; } = 1;
        public Guid InvoiceId { get; set; }
        public InvoiceDto Invoice { get; set; } = new InvoiceDto();
        public double ProductAmount { get; set; }
        public string ProductName => Product?.Name ?? string.Empty;
        public string ProductDescription => Product?.Description ?? string.Empty;
        [DisplayFormat(DataFormatString ="{0:C}")]
        public double ProductPrice => Product?.Price ?? default;
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double TotalCost => (ProductAmount)*Quantity;
    }
}
