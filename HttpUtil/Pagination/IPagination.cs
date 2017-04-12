using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil.Pagination
{
    public interface IPagination
    {
        int QueryCount { get; }
        int Page { get; }
        int ItemsPerPage { get; }
    }
}
