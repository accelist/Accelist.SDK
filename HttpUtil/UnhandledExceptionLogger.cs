using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace TAM.Storage.WebService
{
    public class UnhandledExceptionLogger : ExceptionLogger
    {
        /// <summary>
        /// Your custom exception handler. Action parameters are exception string and request context string respectively.
        /// </summary>
        public readonly Action<string, string> LogHandler;

        /// <summary>
        /// Grabs an exception that triggers in an ASP.NET Web API controller. 
        /// Accepts action with two string parameters, which are exception string and request context string respectively.
        /// </summary>
        /// <param name="logHandler"></param>
        public UnhandledExceptionLogger(Action<string, string> logHandler)
        {
            this.LogHandler = logHandler;
        }

        /// <summary>
        /// Grabs an exception that triggers in an ASP.NET Web API controller, ignores HttpResponseException.
        /// Compiles exception message and stack trace into a string. 
        /// If exception is an AggregateException, all inner exceptions are flattened and then also compiled.
        /// Compiles request context into string, which includes method, URI, header and body.
        /// Pass the compiled exception and request string to the log handler.
        /// </summary>
        /// <param name="context"></param>
        public override void Log(ExceptionLoggerContext context)
        {
            if (context.Exception is HttpResponseException) return;

            //Request body string reader is a promise, let it start as soon as possible.
            var requestBody = context.Request.Content.ReadAsStringAsync();

            var exception = new StringBuilder();
            exception.AppendLine(context.Exception.Message + "\n" + context.Exception.StackTrace);

            var ae = context.Exception as AggregateException;
            if (ae != null)
            {
                foreach (var e in ae.Flatten().InnerExceptions)
                {
                    exception.AppendLine(e.Message + "\n" + e.StackTrace);
                }
            }

            var request = new StringBuilder();
            request.Append(context.Request.Method.Method + " " + context.Request.RequestUri.AbsoluteUri + "\n");
            foreach (var h in context.Request.Headers)
            {
                request.Append(h.Key + " = ");
                foreach (var v in h.Value)
                {
                    request.Append(v + ";");
                }
                request.Append("\n");
            }
            request.Append("\n" + requestBody.Result);

            LogHandler.Invoke(exception.ToString(), request.ToString());
        }
    }
}