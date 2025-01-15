using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ClientManagement.BusinessLogicLayer.Interfaces.Base;
using ClientManagement.DataAccessLayer;
using ClientManagement.DataAccessLayer.Entities.Base;
using Core.Presentation.Models.DataTransferObjects.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Metadata.Ecma335;

namespace ClientManagement.BusinessLogicLayer.Services.Base
{
    public class GenericService<TDto, TEntity> : IGenericService<TDto, TEntity> where TEntity : BaseEntity where TDto : BaseDto
    {
        protected readonly WebDbContext _dbContext;
        protected readonly DbSet<TEntity> _entitySet;
        protected readonly IMapper _mapper;
        public GenericService(WebDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _entitySet = _dbContext.Set<TEntity>();
            _mapper = mapper;
       
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
            var toBeArchived = this._entitySet.AsNoTracking().Where(x => identifiers.Contains(x.Id))
                .ToList();
                toBeArchived.ForEach(x => {
                    x.Archived = true;
                });
            this._entitySet.UpdateRange(toBeArchived);
            return (await this._dbContext.SaveChangesAsync()) > 0;
        }

        public virtual async Task<bool> Delete(IEnumerable<Guid> identifiers)
        {
            var removed = _entitySet.Where(x => identifiers.Contains(x.Id)).ToList();
            _entitySet.RemoveRange(removed);
            return await _dbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<IEnumerable<TDto>> Get(Expression<Func<TEntity, bool>> filter)
        {
            var list = await _entitySet.Where(filter).AsNoTracking().OrderByDescending(x=> x.CreatedOn).ToListAsync();
            return _mapper.Map<List<TDto>>(list);
        }

        public virtual async Task<IEnumerable<TDto>> Get(TDto filter)
        {
            var query = _entitySet.AsNoTracking()
                .Where(x => x.Archived == filter.Archived);
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
            _entitySet.AddRange(toBeInserted);
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
