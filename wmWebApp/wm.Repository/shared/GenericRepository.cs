using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int Start = -1, int Length = -1);
        IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int Start = -1, int Length = -1);
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType = "asc");

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

        public virtual void Save()
        {
            _entities.SaveChanges();
        }

        private IEnumerable<TEntity> GetWithSource(IQueryable<TEntity> source,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int Start = -1, int Length = -1)
        {
            IQueryable<TEntity> query = source;

            if (filter != null)
            {
                query = query.Where(filter);
            }


            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            IOrderedQueryable<TEntity> orderedQuery = null;
            if (orderBy != null)
            {
                orderedQuery = orderBy(query);
            }
            else
            {
                orderedQuery = query.OrderBy(s => 0);
            }


            if (Start >= 0)
            {
                query = orderedQuery.Skip(Start);
            }
            else
            {
                query = orderedQuery.AsQueryable();
            }

            if (Length > 0)
            {
                query = query.Take(Length);
            }
            return query.AsEnumerable<TEntity>();
        }


        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int Start = -1, int Length = -1)
        {
            return this.GetWithSource(_dbset, filter, orderBy, includeProperties, Start, Length);
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "",
    int Start = -1, int Length = -1)
        {
            return this.GetWithSource(_dbset.AsNoTracking(), filter, orderBy, includeProperties, Start, Length);
        }

        //http://stackoverflow.com/questions/16009694/dynamic-funciqueryabletentity-iorderedqueryabletentity-expression
        public Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType = "asc")
        {
            orderType = orderType.ToLower();

            Type typeQueryable = typeof(IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof(TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof(Queryable), methodName, new Type[] { typeof(TEntity), type }, outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>)finalLambda.Compile();
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
