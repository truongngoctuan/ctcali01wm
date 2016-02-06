using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        void Update(T entity);
        void AddOrUpdate(T entity);

        
    }

    public abstract class EntityService<T> : IEntityService<T> where T : BaseEntity
    {
        IUnitOfWork _unitOfWork;
        IGenericRepository<T> _repository;

        public EntityService(IUnitOfWork unitOfWork, IGenericRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _unitOfWork.Commit();
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }
        public virtual void AddOrUpdate(T entity)
        {
            _repository.AddOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return _repository.Get(null, null, include);
        }

    }

    public interface IEntityIntKeyService<T> : IService
    where T : Entity<int>
    {
        void Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(string include = "");
        //Employee GetById(int Id, string include = "");
        void Update(T entity);
        void AddOrUpdate(T entity);
        T GetById(int id, string include = "");
    }

    public abstract class EntityIntKeyService<T> : IEntityIntKeyService<T> where T : Entity<int>
    {
        IUnitOfWork _unitOfWork;
        IGenericIntKeyRepository<T> _repository;

        public EntityIntKeyService(IUnitOfWork unitOfWork, IGenericIntKeyRepository<T> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public virtual void Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _repository.Add(entity);
            _unitOfWork.Commit();
        }

        public virtual void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Edit(entity);
            _unitOfWork.Commit();
        }
        public virtual void AddOrUpdate(T entity)
        {
            _repository.AddOrUpdate(entity);
        }

        public virtual void Delete(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            _repository.Delete(entity);
            _unitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return _repository.Get(null, null, include);
        }
        public T GetById(int id, string include = "")
        {
            return _repository.GetById(id, include);
        }
    }
}
