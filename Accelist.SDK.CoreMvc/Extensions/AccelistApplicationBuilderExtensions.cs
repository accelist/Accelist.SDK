using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.SDK.CoreMvc.Extensions;
using Accelist.SDK.CoreMvc.JWT;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

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
            return app.UseExceptionHandler(configure =>
            {
                configure.UseMiddleware<ApiExceptionHandlerMiddleware>(debugMode);
            });
        }

        /// <summary>
        /// Use plain-text exception handler for web service routes starting with /api, and HTML exception handler for other routes.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="debugMode"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDualExceptionHandlers(this IApplicationBuilder app, bool debugMode)
        {
            app.UseWhen(context => context.IsApiContext(), builder =>
            {
                builder.UseApiExceptionHandler(debugMode);
            });

            app.UseWhen(context => context.IsApiContext() == false, builder =>
            {
                if (debugMode)
                {
                    builder.UseDeveloperExceptionPage();
                }
                else
                {
                    builder.UseExceptionHandler("~/error");
                    builder.UseStatusCodePagesWithReExecute("~/error");
                }
            });

            return app;
        }

        /// <summary>
        /// Adds secured headers so the application will rank A in accordance to https://securityheaders.io recommendations.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecuredHeaders(this IApplicationBuilder app, SecuredHeaderOptions options = null)
        {
            if (options == null)
            {
                options = new SecuredHeaderOptions();
            }

            return app.Use(async (context, next) =>
            {
                if (context.Request.IsHttps)
                {
                    // Enable HSTS
                    var hsts = new TimeSpan(365, 0, 0, 0);
                    var maxAge = Convert.ToInt32(hsts.TotalSeconds);
                    context.Response.Headers.Add("Strict-Transport-Security", $"max-age={maxAge}");
                }

                context.Response.Headers.Add("Content-Security-Policy", options.SerializeContentSecurityPolicy());
                context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("Referrer-Policy", "same-origin");

                //Expect CT: https://scotthelme.co.uk/a-new-security-header-expect-ct/
                await next();
            });
        }

        /// <summary>
        /// Adds bearer token authenticatication using JOSE-JWT custom claims.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="authenticationBuilder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJoseJwt<T>(this AuthenticationBuilder authenticationBuilder,
            string authenticationScheme, Action<JwtAuthenticationOptions> configureOptions)
            where T : StandardTokenClaims
        {
            Jose.JWT.DefaultSettings.JsonMapper = new JoseNewtonsoftMapper();
            return authenticationBuilder.AddScheme<JwtAuthenticationOptions, JwtAuthenticationHandler<T>>(authenticationScheme, configureOptions);
        }

        /// <summary>
        /// Adds bearer token authenticatication using JOSE-JWT standard claims.
        /// </summary>
        /// <param name="authenticationBuilder"></param>
        /// <param name="authenticationScheme"></param>
        /// <param name="configureOptions"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddJoseJwt(this AuthenticationBuilder authenticationBuilder,
            string authenticationScheme, Action<JwtAuthenticationOptions> configureOptions)
        {
            return authenticationBuilder.AddJoseJwt<StandardTokenClaims>(authenticationScheme, configureOptions);
        }
    }
}
