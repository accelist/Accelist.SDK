using Microsoft.ApplicationInsights;
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
        private readonly TelemetryClient Telemetry;

        /// <summary>
        /// Constructs an API exception filter. 
        /// If isDevelopment value is TRUE, will cause full error message + stack traces to be returned in the error response.
        /// If useApplicationInsights is TRUE, will cause the exception to be logged into the ApplicationInsights telemetry.
        /// </summary>
        /// <param name="isDevelopment"></param>
        /// <param name="useApplicationInsights"></param>
        public ApiExceptionFilter(bool isDevelopment, bool useApplicationInsights)
        {
            this.IsDevelopment = isDevelopment;
            if (useApplicationInsights)
            {
                this.Telemetry = new TelemetryClient();
            }
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

            // Because stupid Application Insights doesn't track API exceptions if we return an object result.
            Telemetry?.TrackException(context.Exception);

            var message = IsDevelopment ? ExtractExceptionMessage(context.Exception) : "An error has occurred.";
            context.Result = new ObjectResult(message)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
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
