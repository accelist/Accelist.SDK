using Serilog;
using Serilog.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil
{
    /// <summary>
    /// Extension method for adding Serilog sink for Accelist Logger.
    /// </summary>
    public static class AccelistLoggerConfigurationExtension
    {
        /// <summary>
        /// Adds a sink that sends log via HTTP to a remote logging server, with provided App ID in the request header.
        /// </summary>
        /// <param name="writeTo"></param>
        /// <param name="appId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static LoggerConfiguration Accelist(this LoggerSinkConfiguration writeTo, string appId, string url = "https://log.accelist.com")
        {
            var client = new AccelistLoggerClient(appId);
            return writeTo.Http(url, new Serilog.Sinks.Http.Options(), httpClient: client);
        }
    }
}
