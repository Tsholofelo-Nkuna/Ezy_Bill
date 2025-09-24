using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Services;

using ClientManagement.Presentation.Web.Controllers.Base;
using Core.Presentation.Models.DataTransferObjects;
using Core.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceProductsController : ApiBaseController<InvoiceProductsController>
    {
      //  private readonly ILogger<InvoiceProductsController> _logger;
        private readonly IInvoiceProductService _invoiceProductService;
        private readonly IInvoiceService _invoiceService;
        private readonly IProductService _productService;
     //   private readonly ControllerRequestHandler<InvoiceProductsController>  _requestHandler;

        public InvoiceProductsController(ILogger<InvoiceProductsController> logger, 
            IInvoiceProductService invoiceProductService,
            IInvoiceService invoiceService,
            IProductService productService): base(logger)
        {
           
            _invoiceProductService = invoiceProductService;
         
            _invoiceService = invoiceService;
            _productService = productService;
        }

        [HttpPost("Get")]
        public async Task<PageResponseDto<InvoiceProductDto>> GetInvoiceProducts(PageRequestDto<InvoiceProductDto> pageRequest)
        {
            return await this.requestHandler.HandleRequest(
                () => this.Get(pageRequest, _invoiceProductService),
                nameof(GetInvoiceProducts),
                Task.FromResult<PageResponseDto<InvoiceProductDto>>(new()),
                pageRequest
                );
        }

        // POST api/<InvoiceProductsController>/AddOrUpdate
        [HttpPost]
        public async Task<bool> Post([FromBody] InvoiceProductDto value)
        {
            //Investigate why this method can't be invoked by client, even though client
            //passes it a valid argument
            return await requestHandler.HandleRequest(
                async () =>
                {
                    var invoiceRequest = new PageRequestDto<InvoiceDto> { Filters = new InvoiceDto { Id = value.InvoiceId } };
                    var produceRequest = new PageRequestDto<ProductDto> { Filters = new ProductDto { Id = value.ProductId } };
                    var invoice =  (await _invoiceService.Get(invoiceRequest )).Items.FirstOrDefault();
                    var product = (await _productService.Get(produceRequest)).Items.FirstOrDefault();
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
            return await requestHandler.HandleRequest(
                 async () => {
                     var pRequest = new PageRequestDto<InvoiceProductDto> { Filters = new InvoiceProductDto { Id = invoiceProductId } };
                     var updatedInvoiceProduct = (await _invoiceProductService.Get(pRequest)).Items.FirstOrDefault();
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
            return await requestHandler.HandleRequest(
                async () => await this._invoiceProductService.Delete(new[] {id}),
                nameof(Delete),
                Task.FromResult(false),
                id
                );
        }
    }
}
