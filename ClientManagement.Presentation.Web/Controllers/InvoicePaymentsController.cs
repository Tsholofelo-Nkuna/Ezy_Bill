using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.Presentation.Models.DataTransferObjects;
using ClientManagement.Presentation.Web.Controllers.Base;

using Microsoft.AspNetCore.Mvc;


namespace ClientManagement.Presentation.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicePaymentsController : ApiBaseController<InvoicePaymentsController>
    {
        private readonly IInvoicePaymentService _invoicePaymentService;
        private readonly IInvoiceService _invoiceService;
        public InvoicePaymentsController(ILogger<InvoicePaymentsController> logger, IInvoicePaymentService invoicePaymentService, IInvoiceService invoiceService) : base(logger)
        {
            _invoicePaymentService = invoicePaymentService;
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Invoice identifier</param>
        /// <param name="payment"></param>
        /// <returns></returns>
        [HttpPost()]
       
        public async Task<bool> AddOrUpdatePaymentToInvoice([FromBody] InvoicePaymentDto payment)
        {
            return await this.requestHandler.HandleRequest(
                 async () =>
                 {
                     var targetInvoice =  (await _invoiceService.Get(new InvoiceDto { Id = payment.InvoiceId })).FirstOrDefault();
                     if(targetInvoice is InvoiceDto validInvoiceDto)
                     {
                         payment.Invoice = validInvoiceDto;
                         var serviceResponse = await this._invoicePaymentService.AddPaymentToInvoice(payment.InvoiceId,payment );
                         return serviceResponse is not null;
                     }
                     else
                     {
                         return false;
                     }
                    
                 },
                 nameof(AddOrUpdatePaymentToInvoice),
                 Task.FromResult(false),
                 payment
                );
        }

        [HttpPost("Get")]
        public async Task<IEnumerable<InvoicePaymentDto>> GetInvoicePayments(InvoicePaymentDto filter) {
            return await this.requestHandler.HandleRequest(
                 async () => await this._invoicePaymentService.Get(filter),
                 nameof(GetInvoicePayments),
                 Task.FromResult(Enumerable.Empty<InvoicePaymentDto>()),
                 filter);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await requestHandler.HandleRequest(
                 async () =>  await _invoicePaymentService.Delete(new[] { id}),
                 nameof(Delete),
                 Task.FromResult(false),
                 id
                );
        }
    }
}
