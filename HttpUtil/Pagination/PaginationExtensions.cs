using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil.Pagination
{
    /// <summary>
    /// Extension methods for pagination features.
    /// </summary>
    public static class PaginationExtensions
    {
        /// <summary>
        /// Returns the total page in the pagination.
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static int TotalPage(this IPagination pagination)
        {
            return (int)Math.Ceiling((double)pagination.QueryCount / pagination.ItemsPerPage);
        }

        /// <summary>
        /// Returns the leftmost page number of the pagination using the defined page span.
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static int PageSpanFrom(this IPagination pagination)
        {
            var result = pagination.Page - pagination.PageSpan;
            return (result < 1) ? 1 : result;
        }

        /// <summary>
        /// Returns the rightmost page number of the pagination, using the defined page span.
        /// </summary>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public static int PageSpanTo(this IPagination pagination)
        {
            var result = pagination.Page + pagination.PageSpan;
            var total = pagination.TotalPage();
            return (result > total) ? total : result;
        }
    }
}
