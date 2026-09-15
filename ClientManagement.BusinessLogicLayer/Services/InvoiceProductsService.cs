using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;

using ClientManagement.Models.DataTransferObjects;
using ClientManagement.Utils.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class InvoiceProductsService : GenericService<InvoiceProductDto, InvoiceProductsEntity>, IInvoiceProductService
    {
        public InvoiceProductsService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
        }

        public override async Task<(IEnumerable<InvoiceProductDto> Items, int TotalRecords)> Get(PageRequestDto<InvoiceProductDto> pageRequest)
        {
            var query = base.GetQueryable(pageRequest.Filters);
               
            if(pageRequest.Filters.InvoiceId != Guid.Empty)
            {
                query = query.Where(x => x.Invoice.Id == pageRequest.Filters.InvoiceId);
            }  

            query = query.Include(x => x.Product)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Client);
            var pagedQuery =  !pageRequest.GetAllPages ? query.Skip(pageRequest.PageSize * pageRequest.PageIndex).Take(pageRequest.PageSize) : query;
            var list = await pagedQuery.ToListAsync();
            var results = _mapper.Map<List<InvoiceProductDto>>(list);
            return (results, query.Count());
        }

        public override Task<IEnumerable<InvoiceProductDto>> Update(List<InvoiceProductDto> updates)
        {
            return base.Update(updates);
        }
    }
}
