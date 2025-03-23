using AutoMapper;
using ClientManagement.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer.Services.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.Presentation.Models.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ClientManagement.BusinessLogicLayer.Models;
using Core.Utils.Interfaces;
using ClientManagement.BusinessLogicLayer.Interfaces;
using Core.Presentation.Models.DataTransferObjects;

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ProductService : GenericService<ProductDto, ProductEntity>, IProductService
    {
        public ProductService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
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
    }
}
