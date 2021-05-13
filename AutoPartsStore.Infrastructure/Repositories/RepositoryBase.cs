using AutoPartsStore.Persistence;
using Microsoft.EntityFrameworkCore;
using AutoPartsStore.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AutoPartsStore.Infrastructure.Repositories
{
    public class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<TEntity> _entity;
        public RepositoryBase(ApplicationDbContext context)
        {
            _context = context;
            _entity = context.Set<TEntity>();

        }
        public async Task CreateAsync(TEntity entity)
        {
            await _entity.AddAsync(entity);
        }

        public async Task CreateRangeAsync(IEnumerable<TEntity> entities)
        {
            await _entity.AddRangeAsync(entities);
        }

        public void Delete(TEntity entity)
        {
            _entity.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            _entity.RemoveRange(entities);
        }

        public IEnumerable<TEntity> FindAll(bool isNoTracking = true)
        {
            if (isNoTracking)
                return _entity.AsNoTracking().ToList();
            return _entity.ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(bool isNoTracking = true)
        {
            if (isNoTracking)
                return await _entity.AsNoTracking().ToListAsync();
            return await _entity.ToListAsync();
        }
        public async Task<IEnumerable<TEntity>> FindByConditionAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _entity;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.ToListAsync();
        }

        public async Task<TEntity> FindByIDAsync(object id)
        {
            return await _entity.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllCollectionPropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, IEnumerable<object>>> collection)
        {
            List<TEntity> formatedEntities = new List<TEntity>();
            foreach (var entity in entities)
            {
                await _context.Entry(entity).Collection(collection).LoadAsync();
                formatedEntities.Add(
                    _context.Entry(entity).Entity
                    );
            }
            return formatedEntities;
        }

        public Task<IEnumerable<TEntity>> GetAllCollectionPropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, IEnumerable<object>>>[] collections)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TEntity>> GetAllReferencePropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>>[] references)
        {
            List<TEntity> formatedEntities = new List<TEntity>();
            foreach (var item in entities)
            {
                foreach (var reference in references)
                {
                    await _context.Entry(item).Reference(reference).LoadAsync();
                }
                formatedEntities.Add(
                    _context.Entry(item).Entity
                    );
            }
            return formatedEntities;
        }

        public async Task<IEnumerable<TEntity>> GetAllReferencePropertyAsync(IEnumerable<TEntity> entities, Expression<Func<TEntity, object>> reference)
        {
            List<TEntity> formatedEntities = new List<TEntity>();
            foreach (var entity in entities)
            {
                await _context.Entry(entity).Reference(reference).LoadAsync();
                formatedEntities.Add(
                    _context.Entry(entity).Entity
                    );
            }
            return formatedEntities;
        }

        public async Task<TEntity> GetCollectionPropertyAsync(TEntity entity, Expression<Func<TEntity, IEnumerable<object>>> collection)
        {
            await _context.Entry(entity).Collection(collection).LoadAsync();
            return _context.Entry(entity).Entity;
        }

        public async Task<TEntity> GetCollectionPropertyAsync(TEntity entity, Expression<Func<TEntity, IEnumerable<object>>>[] collections)
        {
            foreach (var item in collections)
            {
                await _context.Entry(entity).Collection(item).LoadAsync();
            }
            return _context.Entry(entity).Entity;
        }

        public int GetCount()
        {
            return _entity.AsNoTracking().Count();
        }

        public int GetCount(Expression<Func<TEntity, bool>> expression)
        {
            return _entity.AsNoTracking().Count(expression);
        }

        public async Task<List<TEntity>> GetPaginateResultAsync(int CurrentPage, int take = 4)
        {
            int skip = (CurrentPage - 1) * take;
            return await _entity.Skip(skip).Take(take).ToListAsync();
        }

        public async Task<TEntity> GetReferencePropertyAsync(TEntity entity, Expression<Func<TEntity, object>> reference)
        {
            await _context.Entry(entity).Reference(reference).LoadAsync();
            return _context.Entry(entity).Entity;
        }

        public async Task<TEntity> GetReferencePropertyAsync(TEntity entity, Expression<Func<TEntity, object>>[] references)
        {
            var ent = _context.Entry(entity);
            foreach (var refrence in references)
            {
                await ent.Reference(refrence).LoadAsync();
            }
            return ent.Entity;
        }

        public void Update(TEntity entity)
        {
            _entity.Update(entity);
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            _entity.UpdateRange(entities);
        }
    }
}
