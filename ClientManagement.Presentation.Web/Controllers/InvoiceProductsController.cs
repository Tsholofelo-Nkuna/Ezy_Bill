using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Services;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Utils;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceProductsController : ControllerBase
    {
        private readonly ILogger<InvoiceProductsController> _logger;
        private readonly IInvoiceProductService _invoiceProductService;
        private readonly IInvoiceService _invoiceService;
        private readonly ProductService _productService;
        private readonly ControllerRequestHandler<InvoiceProductsController>  _requestHandler;

        public InvoiceProductsController(ILogger<InvoiceProductsController> logger, 
            IInvoiceProductService invoiceProductService,
            IInvoiceService invoiceService,
            ProductService productService)
        {
            _logger = logger;
            _invoiceProductService = invoiceProductService;
            _requestHandler = new ControllerRequestHandler<InvoiceProductsController>(_logger);
            _invoiceService = invoiceService;
            _productService = productService;
        }


        // POST api/<InvoiceProductsController>/AddOrUpdate
        [HttpPost]
        public async Task<bool> Post([FromBody] InvoiceProductDto value)
        {
            //Investigate why this method can't be invoked by client, even though client
            //passes it a valid argument
            return await _requestHandler.HandleRequest(
                async () =>
                {
                    var invoice =  (await _invoiceService.Get( new InvoiceDto { Id = value.InvoiceId } )).FirstOrDefault();
                    var product = (await _productService.Get(new ProductDto { Id = value.ProductId })).FirstOrDefault();
                    value.Invoice = invoice ?? new InvoiceDto();
                    value.Product = product ?? new ProductDto();
                    value.ProductAmount = value.Product.Price;
                    if(value.Invoice.Id != Guid.Empty && value.Product.Id != Guid.Empty)
                    {
                       var updates  = await this._invoiceProductService.Update(new() { value });
                        return (updates is not null && updates.Any());
                    }
                    else
                    {
                        return false;
                    }
                },
                nameof(Post),
                Task.FromResult(false),
                value
                );
        }

        [HttpGet("[action]/{invoiceProductId}")]
        public async Task<bool> UpdateInvoiceProductQuantity(Guid invoiceProductId, int quantity)
        {
            return await _requestHandler.HandleRequest(
                 async () => {
                     var updatedInvoiceProduct = (await _invoiceProductService.Get(new InvoiceProductDto { Id = invoiceProductId})).FirstOrDefault();
                     if(updatedInvoiceProduct is not null)
                     {
                         updatedInvoiceProduct.Quantity = quantity;
                         var updated =  (await this._invoiceProductService.Update(new List<InvoiceProductDto> { updatedInvoiceProduct })).FirstOrDefault();
                         return (updated is not null ? updated.Quantity == quantity : false);
                     }
                     else
                     {
                         return false;
                     }
                },
                nameof(UpdateInvoiceProductQuantity),
                Task.FromResult(false),
                invoiceProductId,
                quantity
                );
        }

        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await _requestHandler.HandleRequest(
                async () => await this._invoiceProductService.Delete(new[] {id}),
                nameof(Delete),
                Task.FromResult(false),
                id
                );
        }
    }
}
