using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ClientManagement.BusinessLogicLayer.Interfaces.Base
{
    public interface IGenericService<TDto, TEntity> 
    {
        Task<IEnumerable<TDto>> Get(Expression<Func<TEntity, bool>> filter);
        Task<IEnumerable<TDto>> Get(TDto filter);
        Task<bool> Delete(IEnumerable<Guid> identifiers);
        Task<IEnumerable<TDto>> Update(List<TDto> updates);
        Task<IEnumerable<TDto>> Insert(List<TDto> inserted);
        Task<bool> AddOrUpdate(List<TDto> payload);
        Task<bool> Archive(IEnumerable<Guid> identifiers);
        IQueryable<TEntity> GetQueryable(TDto filter);
    }
}
