using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accelist.WebUtilities.Pagination
{
    /// <summary>
    /// Contains the basic properties required for search pagination.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="QueryType"></typeparam>
    public class BasicPagination<T, QueryType> : IPagination
    {
        /// <summary>
        /// Sets or gets the sequence of items in the current page.
        /// </summary>
        public List<T> PagedItems { set; get; }

        /// <summary>
        /// Sets or gets the current page number.
        /// </summary>
        public int Page { set; get; }

        /// <summary>
        /// Sets or gets the number of how many pages displayed to the left and to the right of current page, in the pagination UI component. Defaults to 4.
        /// </summary>
        public int PageSpan { set; get; } = 4;

        /// <summary>
        /// Sets or gets the search query.
        /// </summary>
        public QueryType Query { set; get; }

        /// <summary>
        /// Sets or gets the total number of items using the search query.
        /// </summary>
        public int QueryCount { set; get; }

        /// <summary>
        /// Sets or gets the number of items per page.
        /// </summary>
        public int ItemsPerPage { set; get; }
    }
}
