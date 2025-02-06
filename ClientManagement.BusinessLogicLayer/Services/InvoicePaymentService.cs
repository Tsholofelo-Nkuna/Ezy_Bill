using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class InvoicePaymentService : GenericService<InvoicePaymentDto, InvoicePaymentEntity>, IInvoicePaymentService
    {
        public InvoicePaymentService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager) : base(dbContext, mapper, httpContextAccessor, userManager)
        {
        }

        public async Task<InvoicePaymentDto?> AddPaymentToInvoice(Guid invoiceId, InvoicePaymentDto paymentDto)
        {
            var targetInvoice = _dbContext.Invoices.AsNoTracking().Include(i => i.Client)
                .FirstOrDefault(x => !x.Archived && x.Id == invoiceId);
            if (targetInvoice is InvoiceEntity validInvoice) 
            {
               paymentDto.Invoice = _mapper.Map<InvoiceDto>(targetInvoice);
               var updates = await this.Update(new List<InvoicePaymentDto> { paymentDto });
               return updates.FirstOrDefault();
            }
            else
            {
                return default;
            }

        }

        public override async Task<IEnumerable<InvoicePaymentDto>> Get(InvoicePaymentDto filter)
        {
            var query = _entitySet.AsNoTracking();
            if(filter.InvoiceId != Guid.Empty)
            {
                query = query.Where(invPayment => invPayment.Invoice.Id == filter.InvoiceId);
            }
            
            var queryResults = await query.Include(ip => ip.Invoice).ToListAsync();
            var returedResults = _mapper.Map<List<InvoicePaymentDto>>(queryResults);
            returedResults.ForEach(invPayment => { 
             invPayment.InvoiceId = invPayment.Invoice.Id;
            });
            return returedResults;
        }
    }
}
