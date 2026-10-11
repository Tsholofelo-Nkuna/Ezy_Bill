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
using Microsoft.Extensions.Logging;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ProductService : GenericService<ProductDto, ProductEntity>, IProductService
    {
        public ProductService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager, ILogger<ProductService> logger) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager, logger)
        {
            
        }

        public override Task<(IEnumerable<ProductDto> Items, int TotalRecords)> Get(PageRequestDto<ProductDto> pageRequest)
        {
            var pId = this.CurrentProfileId;
            var query = this.GetQueryable(pageRequest.Filters);
      

            if (!string.IsNullOrWhiteSpace(pageRequest.Filters.Name)) {
                query = query.Where(x => x.Name.Contains(pageRequest.Filters.Name));
            }

            var result = !pageRequest.GetAllPages ?  query.OrderByDescending(x => x.CreatedOn).Skip(pageRequest.PageIndex*pageRequest.PageSize).Take(pageRequest.PageSize).ToList() : query.OrderByDescending(x => x.CreatedOn).ToList();

            return  Task.FromResult<(IEnumerable<ProductDto> Items, int TotalRecords)>((this._mapper.Map<List<ProductDto>>(result), query.Count()));
        }

        public override  async Task<bool> Delete(IEnumerable<Guid> identifiers)
        {
            var currentProfileId = this.CurrentProfileId;
            var productsWithInvoices = from invoiceProduct in  this._dbContext.InvoicesProducts
                              .Where(x =>  identifiers.Contains(x.Product.Id) && x.ProfileId == currentProfileId)
                              select invoiceProduct;
            var toBeDeleted = identifiers.Where(id => !productsWithInvoices.Select(ip => ip.Product.Id).Contains(id));
            await base.Delete(toBeDeleted ?? []);
            return (toBeDeleted ?? []).Count() == identifiers.Count();
        }

        public override async Task<bool> AddOrUpdate(List<ProductDto> payload)
        {
            var currentProfileId = this.CurrentProfileId;
            var identifiers = payload.Select(p => p.Id);
            var updatedItems = payload.Where(p => p.Id != Guid.Empty);
            var addedItems = payload.Where(prod => !updatedItems.Any(up => up.Id == prod.Id));
            var productsWithInvoices = from invoiceProduct in this._dbContext.InvoicesProducts
                              .Where(x => identifiers.Contains(x.Product.Id) && x.ProfileId == currentProfileId)
                                       select invoiceProduct;
            var toBeUpdated = payload.Where(prod => 
            !productsWithInvoices.Select(ip => ip.Product.Id).Contains(prod.Id)
            && updatedItems.Any(x => x.Id ==prod.Id) 
            ).ToList();
            await base.AddOrUpdate(toBeUpdated.Concat(addedItems).ToList() ?? []);
            return (toBeUpdated ?? []).Count() == updatedItems.Count();
        }
    }
}
