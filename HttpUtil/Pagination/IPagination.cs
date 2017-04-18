using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil.Pagination
{
    public interface IPagination
    {
        int Page { get; }

        int PageSpan { get; }

        int QueryCount { get; }

        int ItemsPerPage { get; }
    }
}
