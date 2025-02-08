using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using System.Linq.Expressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using ClientManagement.DataAccessLayer.Entities;
using Core.Utils.Interfaces;
using ClientManagement.BusinessLogicLayer.Models;
using Core.Utils.Constants;

namespace ClientManagement.BusinessLogicLayer.Services.Base
{
    public class GenericService<TDto, TEntity> : IGenericService<TDto, TEntity> where TEntity : BaseEntity where TDto : BaseDto
    {
        protected readonly WebDbContext _dbContext;
        protected readonly DbSet<TEntity> _entitySet;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected readonly IMapper _mapper;
        protected readonly UserManager<IdentityUser> _userManager;
        protected readonly IAppStateManager<ApplicationState> _appStateManager;
        public GenericService(
            WebDbContext dbContext, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            UserManager<IdentityUser> userManager,
            IAppStateManager<ApplicationState> appStateManager)
        {
            _dbContext = dbContext;
            _entitySet = _dbContext.Set<TEntity>();
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _appStateManager = appStateManager;

        }

        protected Guid CurrentProfileId
        {
            get
            {
                if((_appStateManager.Get<Guid>(nameof(ApplicationState.ProfileId)) == Guid.Empty) 
                    && this._httpContextAccessor.HttpContext.Request.Headers.TryGetValue(AuthConstants.XApiKey, out var apiKey)
                    && this._userManager.Users.FirstOrDefault(x => x.UserName == apiKey.ToString()) is IdentityUser currentUser)
                {
                    var pId = this._dbContext.UserProfiles
                        .Where(x => x.User.Id == currentUser.Id)
                        .Include(x => x.Profile)
                        .FirstOrDefault()?.Profile?.Id ?? Guid.Empty;
                    _appStateManager.Set(nameof(ApplicationState.ProfileId), pId);
                  
                   
                }
                return _appStateManager.Get<Guid>(nameof(ApplicationState.ProfileId));
            }
        }

      
        public virtual async Task<bool> AddOrUpdate(List<TDto> payload)
        {
           var added = payload.Where(x => x.Id == Guid.Empty);
           var updated = payload.Where(x => x.Id != Guid.Empty);
           var hasUpdated = updated.Any() ? (await this.Update(updated.ToList())).Any() : true;
           var hasAdded = added.Any() ? (await this.Insert(added.ToList())).Any() : true;
           return hasUpdated && hasAdded;
        }

        public virtual async Task<bool> Archive(IEnumerable<Guid> identifiers)
        {
            var toBeArchived = this._entitySet.AsNoTracking().Where(x => identifiers.Contains(x.Id) && x.ProfileId == this.CurrentProfileId)
                .ToList();
                toBeArchived.ForEach(x => {
                    x.Archived = true;
                });
            this._entitySet.UpdateRange(toBeArchived);
            return (await this._dbContext.SaveChangesAsync()) > 0;
        }

        public virtual async Task<bool> Delete(IEnumerable<Guid> identifiers)
        {
            var removed = _entitySet.Where(x => identifiers.Contains(x.Id) && x.ProfileId == this.CurrentProfileId).ToList();
            _entitySet.RemoveRange(removed);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<IEnumerable<TDto>> Get(Expression<Func<TEntity, bool>> filter)
        {
         
            var list = await _entitySet
                .Where(filter)
                .Where(x => x.ProfileId == this.CurrentProfileId).AsNoTracking().OrderByDescending(x=> x.CreatedOn).ToListAsync();
            return _mapper.Map<List<TDto>>(list);
        }

        public virtual async Task<IEnumerable<TDto>> Get(TDto filter)
        {
            var query = _entitySet.AsNoTracking()
                .Where(x => x.Archived == filter.Archived && x.ProfileId == this.CurrentProfileId);
            if(filter.Id != Guid.Empty)
            {
                query = query.Where(x => x.Id == filter.Id);
            }
            var result = await query.ToListAsync();
            return  _mapper.Map<IEnumerable<TDto>>( result);
        }
       

        public virtual async Task<IEnumerable<TDto>> Insert(List<TDto> inserted)
        {

            var toBeInserted = _mapper.Map<List<TEntity>>(inserted);
            toBeInserted.ForEach(x => x.ProfileId = this.CurrentProfileId);
            _entitySet.UpdateRange(toBeInserted);
            var insertSuccess = await _dbContext.SaveChangesAsync() > 0;
            toBeInserted.ForEach(x =>
            {
                _dbContext.Entry(x).State = EntityState.Detached;
            });
            return insertSuccess ? _mapper.Map<List<TDto>>(toBeInserted) : Enumerable.Empty<TDto>();

        }

        public virtual async Task<IEnumerable<TDto>> Update(List<TDto> updates)
        {
            var toBeUpdated = _mapper.Map<List<TEntity>>(updates);
            toBeUpdated.ForEach(x => x.ProfileId = this.CurrentProfileId);
            _entitySet.UpdateRange(toBeUpdated);
            var updated = await _dbContext.SaveChangesAsync() > 0;
            var entityProps = typeof(TEntity).GetProperties();
            toBeUpdated.ForEach(x =>
            {
                _dbContext.Entry(x).State = EntityState.Detached;
                var entityPropsWhoAreEntities = entityProps
                .Where(p => p.GetValue(x) is BaseEntity);
                entityPropsWhoAreEntities.ToList()
                .ForEach(eProps =>
                {
                    _dbContext.Entry(eProps.GetValue(x)!).State = EntityState.Detached;
                });

            });
            return updated ? _mapper.Map<List<TDto>>(toBeUpdated) : Enumerable.Empty<TDto>();
        }
    }
}
