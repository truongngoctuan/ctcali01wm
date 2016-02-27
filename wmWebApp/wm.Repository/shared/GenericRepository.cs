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
    public interface IGenericRepository<TEntity>: IReadOnlyRepository<TEntity> where TEntity : BaseEntity
    {
        TEntity Add(TEntity entity);
        TEntity Delete(TEntity entity);
        void Edit(TEntity entity);
        void Save();
    }

    public abstract class GenericRepository<TEntity> : ReadOnlyRepository<TEntity>
      where TEntity : BaseEntity
    {
        public GenericRepository(DbContext context):base(context)
        {
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _dbset.Add(entity);
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

    public interface IGenericIntKeyRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity<int>
    {
        TEntity GetById(int id, string include = "");
    }

    //primary key using int
    public abstract class GenericIntKeyRepository<TEntity> : GenericRepository<TEntity>
      where TEntity : Entity<int>
    {
        public GenericIntKeyRepository(DbContext context) : base(context)
        {
        }

        public TEntity GetById(int id, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = query.Where(s => s.Id == id);

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.First();
        }
    }
}
