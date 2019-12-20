using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using SmartTrinityApi.Core.Entities.Models;
using SmartTrinityApi.Core.Interfaces.Repository;
using Dapper.Contrib;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Data;

namespace SmartTrinityConsole.Infrastructure.Database.Repositories
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        protected dynamic _dbSet;
        protected dynamic _transaction;
        protected virtual string tableId { get; set; }
        protected virtual string tableName { get; set; }

        public Repository(dynamic dbSet, dynamic transaction)
        {
            _dbSet = dbSet;
            _transaction = transaction;
        }

        public virtual void Add(TEntity entity)
        {
            SqlMapperExtensions.Insert((IDbConnection)_dbSet, entity, (IDbTransaction)_transaction);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            SqlMapperExtensions.Insert((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }

        public virtual IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual TEntity Get(int id)
        {
            TEntity entity = null;
            entity = SqlMapperExtensions.Get<TEntity>((IDbConnection)_dbSet, id, (IDbTransaction)_transaction);
            return entity;
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            IEnumerable<TEntity> entities = new List<TEntity>();


            entities = SqlMapperExtensions.GetAll<TEntity>((IDbConnection)_dbSet, (IDbTransaction)_transaction);
            return entities;
        }

        public virtual void Remove(TEntity entity)
        {
            SqlMapperExtensions.Delete((IDbConnection)_dbSet, new { tableId = entity.Id }, (IDbTransaction)_transaction);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            SqlMapperExtensions.Delete((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }

        public virtual TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            // SqlMapperExtensions.Delete(entities);
            throw new NotImplementedException();
        }

        public virtual void Update(TEntity entity)
        {
            SqlMapperExtensions.Update((IDbConnection)_dbSet, entity, (IDbTransaction)_transaction);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            SqlMapperExtensions.Update((IDbConnection)_dbSet, entities, (IDbTransaction)_transaction);
        }
    }
}