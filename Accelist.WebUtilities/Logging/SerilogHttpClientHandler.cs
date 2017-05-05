using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Accelist.WebUtilities.Logging
{
    public class SerilogHttpClientHandler : DelegatingHandler
    {
        public SerilogHttpClientHandler() : base(new HttpClientHandler())
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var timer = Stopwatch.StartNew();
            var response = await base.SendAsync(request, cancellationToken);

            Log.Information("UltimateHttpClient ({ElapsedMilliseconds}ms) : {Method} {Url} => {StatusCode}",
                timer.ElapsedMilliseconds,
                request.Method,
                request.RequestUri,
                response.StatusCode
            );

            return response;
        }
    }
}
