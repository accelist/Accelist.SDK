using Serilog.Sinks.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace HttpUtil
{
    /// <summary>
    /// HTTP client implementation for sending logs to Accelist log server via Serilog HTTP sink.
    /// </summary>
    public class AccelistLoggerClient : IHttpClient
    {
        static HttpClient Client;

        /// <summary>
        /// Constructs an instance of AccelistLoggerClient using provided App ID parameter.
        /// </summary>
        /// <param name="appId"></param>
        public AccelistLoggerClient(string appId)
        {
            if (Client == null)
            {
                Client = new HttpClient();

                // http://byterot.blogspot.co.id/2016/07/singleton-httpclient-dns.html
                // http://www.nimaara.com/2016/11/01/beware-of-the-net-httpclient/
                // This will set the HTTP’s keep-alive header to false so the socket will be closed after a single request.
                // Prevents the server from running out of socket and killing itself LOL.
                // Add roughly extra 35ms (with long tails, i.e amplifying outliers) to each of your HTTP calls, but lets you safely honor DNS changes.
                Client.DefaultRequestHeaders.ConnectionClose = true;
                Client.DefaultRequestHeaders.Add("AppId", appId);
            }
        }

        /// <summary>
        /// Does nothing because System.Net.Http.HttpClient is not supposed to be disposed after each use!
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Sends a POST request to the specified Uri as an asynchronous operation.
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
