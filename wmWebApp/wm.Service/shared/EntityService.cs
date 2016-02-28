using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using wm.Model;
using wm.Repository;
using wm.Service.Common;

// ReSharper disable once CheckNamespace
namespace wm.Service
{
    public interface IEntityService<T> : IService
        where T : BaseEntity
    {
        ServiceReturn Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(string include = "");
        ServiceReturn Update(T entity);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = "");
    }

    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        protected IUnitOfWork UnitOfWork;
        protected IGenericRepository<T> Repos;

        protected EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            UnitOfWork = unitOfWork;
            Repos = repository;
        }

        public virtual ServiceReturn Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            Repos.Add(entity);
            UnitOfWork.Commit();
            return ServiceReturn.Ok;
        }

        public virtual ServiceReturn Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Repos.Edit(entity);
            UnitOfWork.Commit();

            return ServiceReturn.Ok;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            Repos.Delete(entity);
            UnitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return Repos.Get(null, include);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string include = "")
        {
            return Repos.Get(filter, orderBy, include);
        }
    }

    public interface IEntityIntKeyService<T> : IEntityService<T>
    where T : Entity<int>
    {
        T GetById(int id, string include = "");
    }

    public abstract class EntityIntKeyService<T> : EntityService<T>, IEntityIntKeyService<T> where T : Entity<int>
    {
        protected EntityIntKeyService(IUnitOfWork unitOfWork, IGenericRepository<T> repos): base(unitOfWork, repos)
        {
        }
        
        public T GetById(int id, string include = "")
        {
            return Repos.First((s => s.Id == id), include);
        }
    }
}
