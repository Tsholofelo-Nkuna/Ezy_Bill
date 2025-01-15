using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IInvoicePaymentService : IGenericService<InvoicePaymentDto, InvoicePaymentEntity>
    {
        public Task<InvoicePaymentDto?> AddPaymentToInvoice(Guid invoiceId, InvoicePaymentDto invoicePayment);
    }
}
