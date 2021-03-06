﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using wm.Model;

namespace wm.Service
{
    public interface IReadOnlyService<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Filter(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter);
        IQueryable<TEntity> IncludeProperties(IQueryable<TEntity> query, string includeProperties);
        IOrderedQueryable<TEntity> OrderBy(IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryable<TEntity> SkipTake(IOrderedQueryable<TEntity> orderedQuery, int start, int length);

        IEnumerable<TEntity> GetAll(string includeProperties = "");

        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "");
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int start, int length, string includeProperties = "");
        TEntity First(Expression<Func<TEntity, bool>> filter, string includeProperties = "");
        TEntity First(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "");
        TEntity First(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int start, int length, string includeProperties = "");

        IEnumerable<TEntity> GetAsNoTracking(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "",
            int start = -1, int length = -1);
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> GetOrderBy(string orderColumn, string orderType = "asc");

    }

    public class ReadOnlyService<TEntity> : IReadOnlyService<TEntity> where TEntity : BaseEntity
    {
        protected DbContext _entities;
        protected readonly IDbSet<TEntity> _dbset;

        public ReadOnlyService(DbContext context)
        {
            _entities = context;
            _dbset = context.Set<TEntity>();
        }
        public virtual IEnumerable<TEntity> GetAll(string includeProperties = "")
        {

            return IncludeProperties(_dbset, includeProperties).AsEnumerable<TEntity>();
        }

        #region helpers
        public IQueryable<TEntity> Filter(IQueryable<TEntity> query, Expression<Func<TEntity, bool>> filter)
        {
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query;
        }
        public IQueryable<TEntity> IncludeProperties(IQueryable<TEntity> query, string includeProperties)
        {
            var splitString = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //foreach (var includeProperty in splitString)
            //{
            //    query = query.Include(includeProperty);
            //}

            return splitString.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
        public IOrderedQueryable<TEntity> OrderBy(IQueryable<TEntity> query, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            IOrderedQueryable<TEntity> orderedQuery = null;
            if (orderBy != null)
            {
                orderedQuery = orderBy(query);
            }
            else
            {
                orderedQuery = query.OrderBy(s => 0);
            }

            return orderedQuery;
        }
        public IQueryable<TEntity> SkipTake(IOrderedQueryable<TEntity> orderedQuery, int Start, int Length)
        {
            IQueryable<TEntity> query;
            query = Start >= 0 ? orderedQuery.Skip(Start) : orderedQuery.AsQueryable();

            if (Length > 0)
            {
                query = query.Take(Length);
            }

            return query;
        }
        #endregion

        #region Get function
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);

            return query.AsEnumerable<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);
            IOrderedQueryable<TEntity> orderedQuery = this.OrderBy(query, orderBy);
            query = orderedQuery.AsQueryable();

            return query.AsEnumerable<TEntity>();
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int start, int length, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);
            IOrderedQueryable<TEntity> orderedQuery = this.OrderBy(query, orderBy);
            query = this.SkipTake(orderedQuery, start, length);

            return query.AsEnumerable<TEntity>();
        }
        #endregion

        #region First function
        public TEntity First(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);

            return query.First();
        }

        public TEntity First(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);
            IOrderedQueryable<TEntity> orderedQuery = this.OrderBy(query, orderBy);
            query = orderedQuery.AsQueryable();

            return query.First();
        }

        public TEntity First(Expression<Func<TEntity, bool>> filter, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, int start, int length, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbset;
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);
            IOrderedQueryable<TEntity> orderedQuery = this.OrderBy(query, orderBy);
            query = this.SkipTake(orderedQuery, start, length);

            return query.First();
        }
        #endregion

        public virtual IEnumerable<TEntity> GetAsNoTracking(
    Expression<Func<TEntity, bool>> filter = null,
    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
    string includeProperties = "",
    int start = -1, int length = -1)
        {
            IQueryable<TEntity> query = _dbset.AsNoTracking();
            query = this.Filter(query, filter);
            query = this.IncludeProperties(query, includeProperties);
            IOrderedQueryable<TEntity> orderedQuery = this.OrderBy(query, orderBy);
            query = this.SkipTake(orderedQuery, start, length);

            return query.AsEnumerable<TEntity>();
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
