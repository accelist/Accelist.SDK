using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HttpUtil
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
            if (IsLocalhost(filterContext.HttpContext.Request.Host.Host) == false)
            {
                base.HandleNonHttpsRequest(filterContext);
            }
        }

        /// <summary>
        /// Checks whether current host is a localhost.
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        private bool IsLocalhost(string host)
        {
            return (host == "localhost") || (host == "127.0.0.1") || (host == "::1");
        }
    }
}
