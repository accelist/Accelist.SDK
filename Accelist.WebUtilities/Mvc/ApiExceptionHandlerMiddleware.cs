using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Accelist.WebUtilities.Mvc
{
    /// <summary>
    /// Prevents redirecting user to generic error page for API-type requests, but instead return 500 Internal Server Error response.
    /// </summary>
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly bool _debugMode;

        /// <summary>
        /// Constructs a new ApiExceptionHandlerMiddleware instance.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="loggerFactory"></param>
        /// <param name="debugMode"></param>
        public ApiExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, bool debugMode)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ApiExceptionHandlerMiddleware>();
            _debugMode = debugMode;
        }

        /// <summary>
        /// Attempts log any exception happened during the request and returns 500 Internal Server Error response. The response will contain stack traces if debug mode is enabled.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                // Apparently ILogger handles AggregateException just fine.
                _logger.LogError(0, ex, "An unhandled exception has occurred!");

                // We can't do anything if the response has already started, just abort.
                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("The response has already started, the error handler will not be executed.");
                    throw;
                }

                // https://github.com/aspnet/Diagnostics/blob/dev/src/Microsoft.AspNetCore.Diagnostics/ExceptionHandler/ExceptionHandlerMiddleware.cs
                context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
                context.Response.Headers[HeaderNames.Pragma] = "no-cache";
                context.Response.Headers[HeaderNames.Expires] = "-1";
                context.Response.Headers.Remove(HeaderNames.ETag);

                context.Response.StatusCode = 500;
                context.Response.ContentType = "text/plain";

                var body = "An unhandled exception has occurred!";
                if (_debugMode)
                {
                    body = $"{body}\r\n{ex.ToString()}";
                }
                await context.Response.WriteAsync(body);
            }
        }
    }
}
