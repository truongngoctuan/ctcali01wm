using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using wm.Model;

namespace wm.Repository.Shared
{
    public interface IReadOnlyRepository<TEntity> where TEntity : BaseEntity
    {
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

    public abstract class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> _dbset;

        public ReadOnlyRepository(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<TEntity>();
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
            return GetWithSource(_dbset, filter, orderBy, includeProperties, Start, Length);
        }

        public virtual IEnumerable<TEntity> GetAsNoTracking(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "",
    int Start = -1, int Length = -1)
        {
            return GetWithSource(_dbset.AsNoTracking(), filter, orderBy, includeProperties, Start, Length);
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
}
