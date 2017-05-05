using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.WebUtilities.Mvc;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Extension methods for adding new ASP.NET Core middlewares.
    /// </summary>
    public static class AccelistApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds API Exception Handler to the request execution pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app, bool debugMode)
        {
            return app.UseMiddleware<ApiExceptionHandlerMiddleware>(debugMode);
        }

        /// <summary>
        /// Adds secured headers so the application will rank A in accordance to https://securityheaders.io reccomendations.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecuredHeaders(this IApplicationBuilder app, SecuredHeaderOptions options = null)
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

                ctx.Response.Headers.Add("Content-Security-Policy", SecuredHeaderOptions.Serialize(options));
                ctx.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                ctx.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                ctx.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");

                //https://scotthelme.co.uk/a-new-security-header-referrer-policy/
                ctx.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");

                //next: https://scotthelme.co.uk/a-new-security-header-expect-ct/
                await next();
            });
        }
    }
}
