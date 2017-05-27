using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Accelist.SDK.Logging
{
    /// <summary>
    /// Serilog HTTP sink logging client for Accelist CENTRAL.
    /// </summary>
    public class AccelistLoggingClient : Serilog.Sinks.Http.IHttpClient
    {
        /// <summary>
        /// The one and only HttpClient for this logging sink.
        /// </summary>
        private readonly HttpClient Client;

        /// <summary>
        /// Constructs a new logging sink, using a HttpClient to automatically send the API key in request header and close the socket connnection when done.
        /// </summary>
        /// <param name="apiKey"></param>
        public AccelistLoggingClient(Guid apiKey)
        {
            // Why we're not using the UltimateHttpClient?
            // Because we don't want log for the client that send log. Infinite loop yo.
            // Besides, apparently the class is only instantiated once, so there will be only one HttpClient instance for this logger!

            this.Client = new HttpClient();
            Client.DefaultRequestHeaders.ConnectionClose = true;
            Client.DefaultRequestHeaders.Add("AccelistLogApiKey", apiKey.ToString());
        }

        /// <summary>
        /// No-op. We don't dispose our HttpClient.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Asynchronously sends the response message via POST using the configured client.
        /// </summary>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            return await Client.PostAsync(requestUri, content);
        }
    }
}
