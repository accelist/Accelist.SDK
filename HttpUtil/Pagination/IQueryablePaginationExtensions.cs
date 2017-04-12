using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace System.Linq
{
    public static class IQueryablePaginationExtensions
    {
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
