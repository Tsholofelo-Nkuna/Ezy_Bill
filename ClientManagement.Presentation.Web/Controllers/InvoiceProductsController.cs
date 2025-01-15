using ClientManagement.BusinessLogicLayer.Interfaces;
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
        private readonly ControllerRequestHandler<InvoiceProductsController>  _requestHandler;

        public InvoiceProductsController(ILogger<InvoiceProductsController> logger, IInvoiceProductService invoiceProductService)
        {
            _logger = logger;
            _invoiceProductService = invoiceProductService;
            _requestHandler = new ControllerRequestHandler<InvoiceProductsController>(_logger);
        }


        // POST api/<InvoiceProductsController>/AddOrUpdate
        [HttpPost]
        public Task<bool> Post([FromBody] InvoiceProductDto value)
        {
            //Investigate why this method can't be invoked by client, even though client
            //passes it a valid argument
            return Task.FromResult(false);
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
