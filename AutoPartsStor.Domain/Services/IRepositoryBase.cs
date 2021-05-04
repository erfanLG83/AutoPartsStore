using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoPartsStore.Domain.Services
{
    public interface IRepositoryBase<TEntity> where TEntity:class
    {
        Task<IEnumerable<TEntity>> FindAllAsync(bool isNoTracking = true);
        Task<TEntity> FindByIDAsync(Object id);
        Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllReferencePropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> refrence);
        Task<TEntity> GetReferencePropertyAsync(TEntity entity, Expression<Func<TEntity, object>> refrence);
        Task<IEnumerable<TEntity>> GetAllCollectionPropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, IEnumerable<object>>> collection);
        Task<TEntity> GetCollectionPropertyAsync(TEntity entity, Expression<Func<TEntity, IEnumerable<object>>> collection);
        //array ref || collec

        Task<IEnumerable<TEntity>> GetAllReferencePropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>>[] refrences);
        Task<TEntity> GetReferencePropertyAsync(TEntity entity, Expression<Func<TEntity, object>>[] refrences);
        Task<IEnumerable<TEntity>> GetAllCollectionPropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, IEnumerable<object>>>[] collections);
        Task<TEntity> GetCollectionPropertyAsync(TEntity entity, Expression<Func<TEntity, IEnumerable<object>>>[] collections);


        Task CreateAsync(TEntity entity);
        IEnumerable<TEntity> FindAll(bool isNoTracking = true);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        Task CreateRangeAsync(IEnumerable<TEntity> entities);
        void UpdateRange(IEnumerable<TEntity> entities);
        void DeleteRange(IEnumerable<TEntity> entities);
        Task<List<TEntity>> GetPaginateResultAsync(int CurrentPage, int take = 4);
        int GetCount();
        int GetCount(Expression<Func<TEntity, bool>> expression);

    }
}
