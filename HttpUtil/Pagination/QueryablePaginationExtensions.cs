using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    /// <summary>
    /// Extension methods for Paginating an IQueryable sequence using LINQ.
    /// </summary>
    public static class QueryablePaginationExtensions
    {
        /// <summary>
        /// Paginate the query, by defining the page and items-per-page parameter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <returns></returns>
        public static IQueryable<T> Paginate<T>(this IQueryable<T> query, ref int page, int itemsPerPage)
        {
            if (page < 1)
            {
                page = 1;
            }
            var skip = (page - 1) * itemsPerPage;
            return query.Skip(skip).Take(itemsPerPage);
        }
    }
}
