using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Accelist.SDK.Logging
{
    public class AccelistLoggingClient : Serilog.Sinks.Http.IHttpClient
    {
        private readonly HttpClient Client;

        public AccelistLoggingClient(Guid apiKey)
        {
            // Why we're not using the UltimateHttpClient?
            // Because we don't want log for the client that send log. Infinite loop yo.
            // Besides, apparently the class is only instantiated once, so there will be only one HttpClient instance for this logger!

            this.Client = new HttpClient();
            Client.DefaultRequestHeaders.ConnectionClose = true;
            Client.DefaultRequestHeaders.Add("AccelistLogApiKey", apiKey.ToString());
        }

        public void Dispose()
        {
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await Client.PostAsync(requestUri, content);
        }
    }
}
