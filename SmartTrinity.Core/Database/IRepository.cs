using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SmartTrinity.Core.Database
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> Get(int id);

        Task<IEnumerable<TEntity>> GetAll();

        Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<int> Add(TEntity entity);

        Task<int> AddRange(IEnumerable<TEntity> entities);

        Task<bool> Remove(TEntity entity);

        Task<bool> RemoveRange(IEnumerable<TEntity> entities);

        Task<bool> Update(TEntity entity);

        Task<bool> UpdateRange(IEnumerable<TEntity> entities);
    }
}