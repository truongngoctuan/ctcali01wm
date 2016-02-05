using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);
        TEntity Add(TEntity entity);
        TEntity Delete(TEntity entity);
        void Edit(TEntity entity);
        void Save();
        void AddOrUpdate(TEntity entity);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
    }

    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity>
      where TEntity : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> _dbset;

        public GenericRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {

            return _dbset.AsEnumerable<TEntity>();
        }

        public IEnumerable<TEntity> FindBy(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate)
        {

            IEnumerable<TEntity> query = _dbset.Where(predicate).AsEnumerable();
            return query;
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
            _entities.Entry(entity).State = System.Data.Entity.EntityState.Modified;
        }
        public virtual void AddOrUpdate(TEntity entity)
        {
            _dbset.AddOrUpdate(entity);
        }

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
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

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.First();
        }
    }

    public interface IGenericStringKeyRepository<TEntity> : IGenericRepository<TEntity> where TEntity : Entity<int>
    {
        TEntity GetById(string id, string include = "");
    }

    //primary key using int
    public abstract class GenericStringKeyRepository<TEntity> : GenericRepository<TEntity>
      where TEntity : Entity<int>
    {
        public GenericStringKeyRepository(DbContext context) : base(context)
        {
        }

        public TEntity GetById(string id, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.First();
        }
    }
}
