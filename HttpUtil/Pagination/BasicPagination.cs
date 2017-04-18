using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil.Pagination
{
    public class BasicPagination<T, QueryType> : IPagination
    {
        public List<T> PagedItems { set; get; }

        public int Page { set; get; }

        public int PageSpan { set; get; } = 4;

        public QueryType Query { set; get; }

        public int QueryCount { set; get; }

        public int ItemsPerPage { set; get; }
    }
}
