using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using wm.Model;
using wm.Repository;

namespace wm.Service
{
    public interface IService
    {
    }

    public interface IEntityService<T> : IService
        where T : BaseEntity
    {
        void Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(string include = "");
        //Employee GetById(int Id, string include = "");
        ServiceReturn Update(T entity);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = "");
    }

    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        protected IUnitOfWork _unitOfWork;
        protected IGenericRepository<T> _repos;

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repos = repository;
        }

        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repos.Add(entity);
            _unitOfWork.Commit();
        }

        public virtual ServiceReturn Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repos.Edit(entity);
            _unitOfWork.Commit();

            return ServiceReturn.Ok;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repos.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return _repos.Get(null, include);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
    string include = "")
        {
            return _repos.Get(filter, orderBy, include);
        }
    }

    public interface IEntityIntKeyService<T> : IService
    where T : Entity<int>
    {
        ServiceReturn Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(string include = "");
        //Employee GetById(int Id, string include = "");
        ServiceReturn Update(T entity);
        T GetById(int id, string include = "");

        IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = "");
    }

    public abstract class EntityIntKeyService<T> : IEntityIntKeyService<T> where T : Entity<int>
    {
        protected IUnitOfWork _unitOfWork;
        protected IGenericIntKeyRepository<T> _repos;

        public EntityIntKeyService(IUnitOfWork unitOfWork, IGenericIntKeyRepository<T> repos)
        {
            _unitOfWork = unitOfWork;
            _repos = repos;
        }

        public virtual ServiceReturn Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repos.Add(entity);
            _unitOfWork.Commit();
            return ServiceReturn.Ok;
        }

        public virtual ServiceReturn Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repos.Edit(entity);
            _unitOfWork.Commit();

            return ServiceReturn.Ok;
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repos.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return _repos.Get(null, null, include);
        }
        public T GetById(int id, string include = "")
        {
            return _repos.GetById(id, include);
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter = null,
    Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string include = "")
        {
            return _repos.Get(filter, orderBy, include);
        }
    }
}
