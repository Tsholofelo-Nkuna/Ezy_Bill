using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using Core.Presentation.Models.DataTransferObjects;
using Core.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class InvoicePaymentService : GenericService<InvoicePaymentDto, InvoicePaymentEntity>, IInvoicePaymentService
    {
        public InvoicePaymentService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
        }

        public async Task<InvoicePaymentDto?> AddPaymentToInvoice(Guid invoiceId, InvoicePaymentDto paymentDto)
        {
            var pId = this.CurrentProfileId;
            var targetInvoice = _dbContext.Invoices.AsNoTracking().Where(x => x.ProfileId == pId).Include(i => i.Client)
                .FirstOrDefault(x => !x.Archived && x.Id == invoiceId);
            if (targetInvoice is InvoiceEntity validInvoice) 
            {
               paymentDto.Invoice = _mapper.Map<InvoiceDto>(targetInvoice);
                paymentDto.ProfileId = pId;
                var updates = await this.Update(new List<InvoicePaymentDto> { paymentDto });
               return updates.FirstOrDefault();
            }
            else
            {
                return default;
            }

        }

        public override async Task<(IEnumerable<InvoicePaymentDto> Items, int TotalRecords)> Get(PageRequestDto<InvoicePaymentDto> pageRequest)
        {
            var pId = this.CurrentProfileId;
            var query = this.GetQueryable(pageRequest.Filters);
            var filter = pageRequest.Filters;
            if(filter.InvoiceId != Guid.Empty)
            {
                query = query.Where(invPayment => invPayment.Invoice.Id == filter.InvoiceId);
            }
            var count = query.Count();
            var pagedQuery = !pageRequest.GetAllPages ? query.Include(ip => ip.Invoice).Skip(pageRequest.PageIndex * pageRequest.PageSize).Take(pageRequest.PageSize) : query.Include(ip => ip.Invoice);
            var queryResults = await pagedQuery.ToListAsync();
            var returedResults = _mapper.Map<List<InvoicePaymentDto>>(queryResults);
            returedResults.ForEach(invPayment => { 
             invPayment.InvoiceId = invPayment.Invoice.Id;
            });
            return ((returedResults, count));
        }
    }
}
