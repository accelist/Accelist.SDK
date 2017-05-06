using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Accelist.SDK.CoreMvc.Extensions
{
    /// <summary>
    /// Prevents redirecting user to generic error page for API-type requests, but instead return 500 Internal Server Error response.
    /// </summary>
    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiExceptionHandlerMiddleware> _logger;
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
            var e = context.Features.Get<IExceptionHandlerFeature>();

            context.Response.Clear();
            context.Response.Headers[HeaderNames.CacheControl] = "no-cache";
            context.Response.Headers[HeaderNames.Pragma] = "no-cache";
            context.Response.Headers[HeaderNames.Expires] = "-1";
            context.Response.Headers.Remove(HeaderNames.ETag);

            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";

            var body = "An unhandled exception has occurred!";
            if (_debugMode)
            {
                body = $"{body}\r\n{e.Error.ToString()}";
            }

            // Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware will log the error for us.
            await context.Response.WriteAsync(body);
        }
    }
}
