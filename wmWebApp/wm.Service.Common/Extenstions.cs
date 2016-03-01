using System;
using System.Data.Entity;
using System.Linq;

namespace wm.Service
{
    public static class Extentions
    {
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
}
