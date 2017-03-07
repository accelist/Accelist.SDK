using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using HttpUtil;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// This filter is designed to be a global filter added to MVC Core option for handling non-HTTPS requests when accessed through localhost.
    /// </summary>
    public class RemoteHttpsAttribute : RequireHttpsAttribute
    {
        /// <summary>
        /// This method causes requests performed against non-localhost machines to be redirected into HTTPS. 
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleNonHttpsRequest(AuthorizationFilterContext filterContext)
        {
            if (filterContext.HttpContext.IsLocalhost() == false)
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }
    }
}
