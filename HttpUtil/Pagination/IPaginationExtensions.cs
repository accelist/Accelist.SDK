using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil.Pagination
{
    public static class IPaginationExtensions
    {
        public static int TotalPage(this IPagination pagination)
        {
            return (int)Math.Ceiling((double)pagination.QueryCount / pagination.ItemsPerPage);
        }

        public static int PageSpanFrom(this IPagination pagination)
        {
            var result = pagination.Page - pagination.PageSpan;
            return (result < 1) ? 1 : result;
        }

        public static int PageSpanTo(this IPagination pagination)
        {
            var result = pagination.Page + pagination.PageSpan;
            var total = pagination.TotalPage();
            return (result > total) ? total : result;
        }
    }
}
