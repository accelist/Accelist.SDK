using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace HttpUtil
{
    /// <summary>
    /// This class contains extension methods for rapidly working with Core MVC objects.
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// This extension method allows multiple errors in a ModelState to be quickly synthesized into a single error message string.
        /// Useful for returning error message after a validation failure from an web service endpoint.
        /// </summary>
        /// <param name="modelstate"></param>
        /// <returns></returns>
        public static string JoinErrorMessages(this ModelStateDictionary modelstate)
        {
            var errors = modelstate.Values.SelectMany(Q => Q.Errors).Select(Q => Q.ErrorMessage);
            return string.Join(", ", errors);
        }

        /// <summary>
        /// This extension method checks whether a request path starts with "/api"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsApiContext(this HttpContext context)
        {
            return context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether current request host is a localhost.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsLocalhost(this HttpContext context)
        {
            var host = context.Request.Host.Host;
            return (host == "localhost") || (host == "127.0.0.1") || (host == "::1");
        }

        /// <summary>
        /// Adds secured headers so the site will rank A in accordance to https://securityheaders.io reccomendations.
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecuredHeaders(this IApplicationBuilder app)
        {
            return app.Use(async (ctx, next) =>
            {
                if (ctx.Request.IsHttps)
                {
                    // Enable HSTS
                    var hsts = new TimeSpan(365, 0, 0, 0);
                    var maxAge = Convert.ToInt32(hsts.TotalSeconds);
                    ctx.Response.Headers.Add("Strict-Transport-Security", $"max-age={maxAge}");
                }

                ctx.Response.Headers.Add("Content-Security-Policy", "script-src 'self'");
                ctx.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                ctx.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                await next();
            });
        }
    }
}
