using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using wm.Model;

namespace wm.Service.Common
{
    public interface IDatatablesService<TEntity> where TEntity : BaseEntity
    {
        IEnumerable<TEntity> ListDatatables(Expression<Func<TEntity, bool>> filter, string sortOrder, int start,
            int Length, out int recordsTotal, out int recordsFiltered);
    }

    public class DatatablesService<TEntity> : IDatatablesService<TEntity> where TEntity : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> _dbset;
        public IReadOnlyService<TEntity> ReadOnlyRepository { get; set; }

        public DatatablesService(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<TEntity>();

            ReadOnlyRepository = new ReadOnlyService<TEntity>(context);
        }


        //http://stackoverflow.com/questions/16009694/dynamic-funciqueryableT-iorderedqueryableT-expression
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn,
            string orderType = "asc")
        {
            orderType = orderType.ToLower();

            Type typeQueryable = typeof (IQueryable<TEntity>);
            ParameterExpression argQueryable = Expression.Parameter(typeQueryable, "p");
            var outerExpression = Expression.Lambda(argQueryable, argQueryable);
            string[] props = orderColumn.Split('.');
            IQueryable<TEntity> query = new List<TEntity>().AsQueryable<TEntity>();
            Type type = typeof (TEntity);
            ParameterExpression arg = Expression.Parameter(type, "x");

            Expression expr = arg;
            foreach (string prop in props)
            {
                PropertyInfo pi = type.GetProperty(prop,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                expr = Expression.Property(expr, pi);
                type = pi.PropertyType;
            }
            LambdaExpression lambda = Expression.Lambda(expr, arg);
            string methodName = orderType == "asc" ? "OrderBy" : "OrderByDescending";

            MethodCallExpression resultExp =
                Expression.Call(typeof (Queryable), methodName, new Type[] {typeof (TEntity), type},
                    outerExpression.Body, Expression.Quote(lambda));
            var finalLambda = Expression.Lambda(resultExp, argQueryable);
            return (Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>) finalLambda.Compile();
        }

        public IEnumerable<TEntity> ListDatatables(Expression<Func<TEntity, bool>> filter, string sortOrder, int start,
            int length, out int recordsTotal, out int recordsFiltered)
        {
            var SortOrderSplit = sortOrder.Split(' ');

            var orderFunction = SortOrderSplit.Length == 2
                ? GetOrderBy(SortOrderSplit[0], SortOrderSplit[1])
                : GetOrderBy(SortOrderSplit[0]);

            recordsTotal = _dbset.Count();
            recordsFiltered = _dbset.Count(filter);

            var resulFiltered = ReadOnlyRepository.Get(filter,
                orderFunction, start, length, "Branch");

            return resulFiltered;
        }
    }
}
