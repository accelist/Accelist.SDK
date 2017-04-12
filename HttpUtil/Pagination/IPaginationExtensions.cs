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
    }
}
