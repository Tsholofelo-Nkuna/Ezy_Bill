using AutoMapper;
using ClientManagement.BusinessLogicLayer.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Core.Utils.Interfaces;
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

        public override async Task<IEnumerable<InvoiceProductDto>> Get(InvoiceProductDto filter)
        {
            var query = base.GetQueryable(filter)
                .Include(x => x.Product)
                .Include(x => x.Invoice)
                .ThenInclude(x => x.Client);
              
                
            var list = await query.ToListAsync();
            var results = _mapper.Map<List<InvoiceProductDto>>(list);
            return results;
        }
    }
}
