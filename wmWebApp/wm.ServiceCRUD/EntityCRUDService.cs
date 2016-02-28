using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using wm.Model;
using wm.Repository;
using wm.Service.Common;

// ReSharper disable once CheckNamespace
namespace wm.ServiceCRUD
{
    public static class Extentions
    {//TODO: move to common
        public static IQueryable<T> IncludeProperties<T>(this IQueryable<T> query, string includeProperties)
        {
            var splitString = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var includeProperty in splitString)
            //{
            //    query = query.Include(includeProperty);
            //}

            return splitString.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }

    // ReSharper disable once InconsistentNaming
    public interface IEntityCRUDService<T> : IService
        where T : BaseEntity
    {
        ServiceReturn Create(T entity);
        void Delete(T entity);
        IEnumerable<T> GetAll(string include = "");
        ServiceReturn Update(T entity);
    }

    // ReSharper disable once InconsistentNaming
    public abstract class EntityCRUDService<T> : IEntityCRUDService<T> where T : BaseEntity
    {
        protected IUnitOfWork UnitOfWork;
        //protected IGenericRepository<T> Repos;
        protected DbContext _entities;
        protected readonly IDbSet<T> _dbset;
        protected EntityCRUDService(IUnitOfWork unitOfWork, DbContext context)
        {
            _entities = context;
            _dbset = context.Set<T>();
            UnitOfWork = unitOfWork;
        }

        public virtual ServiceReturn Create(T entity)
        {
            _dbset.Add(entity);
            UnitOfWork.Commit();
            return ServiceReturn.Ok;
        }

        public virtual ServiceReturn Update(T entity)
        {
            _entities.Entry(entity).State = EntityState.Modified;
            UnitOfWork.Commit();
            return ServiceReturn.Ok;
        }

        public virtual void Delete(T entity)
        {
            _dbset.Remove(entity);
            UnitOfWork.Commit();
        }

        public virtual IEnumerable<T> GetAll(string include = "")
        {
            return _dbset.IncludeProperties(include).AsEnumerable();
        }
    }

    // ReSharper disable once InconsistentNaming
    public interface IEntityIntKeyCRUDService<T> : IEntityCRUDService<T>
    where T : Entity<int>
    {
        T GetById(int id, string include = "");
    }

    // ReSharper disable once InconsistentNaming
    public abstract class EntityIntKeyCRUDService<T> : EntityCRUDService<T>, IEntityIntKeyCRUDService<T> where T : Entity<int>
    {
        protected EntityIntKeyCRUDService(IUnitOfWork unitOfWork, DbContext context) : base(unitOfWork, context)
        {
        }

        public T GetById(int id, string include = "")
        {
            return _dbset.IncludeProperties(include).First(s => s.Id == id);
        }
    }
}
