using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Authorization
{
    /// <summary>
    /// This filter allows anonymous user to access the path but bounces an authenticated user to the home page.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class GuestAttribute : Attribute, IActionFilter, IAllowAnonymous
    {
        /// <summary>
        /// If value is provided with a local URL, will redirect user to that URL instead.
        /// If not provided, will attempt to redirect the user to default MVC route: Home/Index
        /// </summary>
        public string RedirectTo { set; get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
        
        /// <summary>
        /// Prevents request to be completed if user is authenticated and instead bounces to the home page.
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                return;
            }

            if (string.IsNullOrEmpty(RedirectTo))
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            else
            {
                context.Result = new LocalRedirectResult(RedirectTo);
            }
        }
    }
}
