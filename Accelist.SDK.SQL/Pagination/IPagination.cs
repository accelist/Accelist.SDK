using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accelist.SDK.SQL.Pagination
{
    /// <summary>
    /// Contains interfaces required for pagination features extension.
    /// </summary>
    public interface IPagination
    {
        /// <summary>
        /// Gets the current page number.
        /// </summary>
        int Page { get; }

        /// <summary>
        /// Gets the number of how many pages displayed to the left and to the right of current page, in the pagination UI component.
        /// </summary>
        int PageSpan { get; }

        /// <summary>
        /// Gets the total number of items using the search query.
        /// </summary>
        int QueryCount { get; }

        /// <summary>
        /// Gets the number of items per page.
        /// </summary>
        int ItemsPerPage { get; }
    }
}
