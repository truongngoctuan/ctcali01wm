using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using wm.Model;
using wm.Repository.Shared;

namespace wm.Repository
{
    public interface IUpdateRepository<TEntity>  where TEntity : BaseEntity
    {
        TEntity Delete(TEntity entity);
        void Edit(TEntity entity);
        void Save();
    }

    public abstract class UpdateRepository<TEntity> : IUpdateRepository<TEntity>
      where TEntity : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> _dbset;

        public UpdateRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<TEntity>();
        }
        
        public virtual TEntity Delete(TEntity entity)
        {
            return _dbset.Remove(entity);
        }

        public virtual void Edit(TEntity entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

    }
}
