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

namespace ClientManagement.BusinessLogicLayer.Services
{
    public class ProductService : GenericService<ProductDto, ProductEntity>
    {
        public ProductService(WebDbContext dbContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager,
              IAppStateManager<ApplicationState> appStateManager) : base(dbContext, mapper, httpContextAccessor, userManager, appStateManager)
        {
            
        }

        public override Task<IEnumerable<ProductDto>> Get(ProductDto filter)
        {
            var pId = this.CurrentProfileId;
            var query = this._entitySet.Where(x => true && x.ProfileId == pId).AsNoTracking();
            if (filter.Archived)
            {
                query = query.Where(x => x.Archived);
            }
            else
            {
               query =  query.Where(x => !x.Archived);
            }

            if (filter.Id != Guid.Empty) { 
              query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrWhiteSpace(filter.Name)) {
                query = query.Where(x => x.Name.Contains(filter.Name));
            }

            var result =  query.OrderByDescending(x => x.CreatedOn).ToList();

            return  Task.FromResult<IEnumerable<ProductDto>>(this._mapper.Map<List<ProductDto>>(result));
        }
    }
}
