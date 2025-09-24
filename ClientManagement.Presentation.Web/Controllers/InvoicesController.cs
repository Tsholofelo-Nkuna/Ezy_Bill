using ClientManagement.BusinessLogicLayer.Interfaces;

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
    public class InvoicesController : ApiBaseController<InvoicesController>
    {
        private readonly IInvoiceService _invoiceService;
      
        public InvoicesController(IInvoiceService invoiceService, ILogger<InvoicesController> logger): base(logger) {
           this._invoiceService = invoiceService;
         
        }
        // GET: api/<InvoicesController>/Get
        [HttpPost("[action]")]
        public async Task<PageResponseDto<InvoiceDto>> Get([FromBody] PageRequestDto<InvoiceDto> pageRequest)
        {
            return  await this.requestHandler.HandleRequest(
                async () => await this.Get(pageRequest, _invoiceService),
                nameof(Get),
                Task.FromResult<PageResponseDto<InvoiceDto>>( new() { Items = Enumerable.Empty<InvoiceDto>() }),
                pageRequest
                );
        }

        // GET api/<InvoicesController>/5
        [HttpGet("{id}")]
        public async Task<InvoiceDto?> Get(Guid id)
        {
            return await this.requestHandler.HandleRequest(
                async () => (await this._invoiceService.Get(new InvoiceDto { Id = id })).FirstOrDefault(),
                nameof(Get),
                Task.FromResult<InvoiceDto?>(null),
                id
                );
        }

        // POST api/<InvoicesController>
        [HttpPost]
        public async Task<bool> Post([FromBody] InvoiceDto value)
        {
            return await this.requestHandler.HandleRequest(
                async () => await this._invoiceService.AddOrUpdate(new List<InvoiceDto> { value }),
                nameof(Post),
                Task.FromResult(false),
                value
               );
        }

        //POST api/<InvoiceController>/?id={id}
        [HttpPost("AddPayment")]
        public async Task<InvoiceDto?> Post([FromQuery] Guid id, [FromBody] double amount)
        {
            return await this.requestHandler.HandleRequest(async () =>
               await this._invoiceService.AddPaymentToInvoice(id, amount),
               nameof(Post),
               Task.FromResult<InvoiceDto?>(null),
               id,
               amount
               );
        }

        //POST api/<InvoiceController>/?id={id}
        [HttpPost("AddProducts")]
        public async Task<InvoiceDto?> Post([FromQuery] Guid id, [FromBody] IEnumerable<Guid> productsIdentifiers)
        {
            return await this.requestHandler.HandleRequest(async () =>
             await this._invoiceService.AddProductsToInvoice(id, productsIdentifiers),
             nameof(Post),
             Task.FromResult<InvoiceDto?>(null),
             id,
             productsIdentifiers
            );
        }
            // PUT api/<InvoicesController>/5
            [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<InvoicesController>/5
        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await this.requestHandler
                .HandleRequest(
                 async () => await this._invoiceService.Delete(new[] {id}),
                 nameof(Delete),
                 Task.FromResult(false),
                 id
                );
        }

        [HttpPost("[action]/{id}")]
        public async Task<bool> Archive(Guid id)
        {
            return await this.requestHandler.HandleRequest(
                async () => await this._invoiceService.Archive(new[] { id }),
                nameof(Archive),
                Task.FromResult(false),
                id);
        }
    }
}
