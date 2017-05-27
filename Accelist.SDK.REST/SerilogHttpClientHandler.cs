using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Accelist.SDK.REST
{
    /// <summary>
    /// 
    /// </summary>
    public class SerilogHttpClientHandler : DelegatingHandler
    {
        /// <summary>
        /// Constructs the HTTP client handler, based on default .NET HTTP client handler.
        /// </summary>
        public SerilogHttpClientHandler() : base(new HttpClientHandler())
        {
        }

        /// <summary>
        /// Sends HTTP request message asynchronously, then logging the elapsed time and response status code using Serilog.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timer = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);

            Log.Information("UltimateHttpClient ({ElapsedMilliseconds}ms) : {Method} {Url} => {StatusCode} {StatusCodeText}",
                timer.ElapsedMilliseconds,
                request.Method,
                request.RequestUri,
                (int)response.StatusCode,
                response.StatusCode
            );

            return response;
        }
    }
}
