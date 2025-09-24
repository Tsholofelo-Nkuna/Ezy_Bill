using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Interfaces
{
    public interface IInvoiceService : IGenericService<InvoiceDto, InvoiceEntity>
    {
        public Task<InvoiceDto?> AddProductsToInvoice(Guid invoiceId, IEnumerable<Guid> productIdentifiers);
        public Task<InvoiceDto?> AddPaymentToInvoice(Guid invoiceId, double amount);
        public Task<InvoiceDto?> CreateInvoice(DateTime dueDate);
    }
}
