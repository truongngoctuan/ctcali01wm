using System;
using System.Data.Entity;
using System.Linq;
using wm.Model;
using wm.Service.Common;

// ReSharper disable once CheckNamespace
namespace wm.Service
{
    public interface IEntityService<T> : IReadOnlyService<T>
        where T : BaseEntity
    {
        ServiceReturn Create(T entity);
        void Delete(T entity);
        ServiceReturn Update(T entity);
    }

    public abstract class EntityService<T> : ReadOnlyService<T>, IEntityService<T> where T : BaseEntity
    {
        protected IUnitOfWork UnitOfWork;
        public DbContext Context { get; set; }
        protected readonly IDbSet<T> _dbset;

        protected EntityService(IUnitOfWork unitOfWork, DbContext context): base(context)
        {
            UnitOfWork = unitOfWork;
            Context = context;
            _dbset = context.Set<T>();
        }

        public virtual ServiceReturn Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _dbset.Add(entity);
            UnitOfWork.Commit();
            return ServiceReturn.Ok;
        }

        public virtual ServiceReturn Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Context.Entry(entity).State = EntityState.Modified;
            UnitOfWork.Commit();

            return ServiceReturn.Ok;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _dbset.Remove(entity);
            UnitOfWork.Commit();
        }
    }

    public interface IEntityIntKeyService<T> : IEntityService<T>
    where T : Entity<int>
    {
        T GetById(int id, string include = "");
    }

    public abstract class EntityIntKeyService<T> : EntityService<T>, IEntityIntKeyService<T> where T : Entity<int>
    {
        protected EntityIntKeyService(IUnitOfWork unitOfWork, DbContext context) : base(unitOfWork, context)
        {
        }
        
        public T GetById(int id, string include = "")
        {
            return _dbset.IncludeProperties(include).First(s => s.Id == id);
        }
    }
}
