using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace HttpUtil
{
    /// <summary>
    /// Provides extension methods for ASP.NET Web API global HttpConfiguration.
    /// </summary>
    public static class HttpConfigurationExtensions
    {
        /// <summary>
        /// Attach a log handler routine to unhandled exceptions in ASP.NET Web API methods.
        /// </summary>
        /// <param name="config"></param>
        /// <param name="logHandler"></param>
        public static void AttachUnhandledExceptionLogger(this HttpConfiguration config, Action<string, string> logHandler)
        {
            config.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger(logHandler));
        }

        /// <summary>
        /// Set JSON formatter as default when returning responses served directly to web browsers (text/html content types).
        /// </summary>
        /// <param name="config"></param>
        public static void SetJsonFormatterAsDefault(this HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/html")
                );
        }
    }
}
