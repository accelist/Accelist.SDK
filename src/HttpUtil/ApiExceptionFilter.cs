using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace HttpUtil
{
    /// <summary>
    /// This exception filter is intended to be a global filter added to the MVC Core options for handling API controller error responses.
    /// </summary>
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly bool IsDevelopment;

        /// <summary>
        /// Constructs an API exception filter. 
        /// If isDevelopment value is TRUE, will cause full error message + stack traces to be returned in the error response.
        /// </summary>
        /// <param name="isDevelopment"></param>
        public ApiExceptionFilter(bool isDevelopment)
        {
            this.IsDevelopment = isDevelopment;
        }

        /// <summary>
        /// This method causes endpoints located on path starting with "/api" to return error string instead of an HTML error page.
        /// </summary>
        /// <param name="context"></param>
        public override void OnException(ExceptionContext context)
        {
            if (context.HttpContext.IsApiContext() == false)
            {
                return;
            }

            var message = IsDevelopment ? ExtractExceptionMessage(context.Exception) : "An unhandled exception has occurred.";
            context.Result = new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };

            // This is done so our error-handling middlewares like ApplicationInsights can actually catch the exception!
            base.OnException(context);
        }

        /// <summary>
        /// This method returns a response string from a given exception.
        /// Development environment will cause full stack trace and exception messages to be returned to the client.
        /// Otherwise, only simple error string to be returned to the client.
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public string ExtractExceptionMessage(Exception exception)
        {
            var aggregated = exception as AggregateException;
            if (aggregated == null)
            {
                return exception.Message + "\n" + exception.StackTrace;
            }
            else
            {
                var messages = aggregated.InnerExceptions.Select(Q => Q.Message + "\n" + Q.StackTrace);
                return string.Join("\n\n", messages);
            }
        }
    }
}
