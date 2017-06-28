using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.SDK.Logging;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.Http.TextFormatters;

namespace Serilog
{
    /// <summary>
    /// Extension method for configuring Serilog against Accelist CENTRAL logging sink.
    /// </summary>
    public static class AccelistSerilogExtensions
    {
        /// <summary>
        /// Sends telemetry logs to Accelist CENTRAL using provided API key.
        /// </summary>
        /// <param name="sinkConfiguration"></param>
        /// <param name="apiKey"></param>
        /// <param name="server"></param>
        /// <param name="minimumSeverity"></param>
        /// <returns></returns>
        public static LoggerConfiguration Accelist(this LoggerSinkConfiguration sinkConfiguration, Guid apiKey, string server = null, LogEventLevel minimumSeverity = LogEventLevel.Information)
        {
            var dom = server?.TrimEnd('/') ?? "https://log.accelist.com";
            
            return sinkConfiguration.Http(dom + "/api/v1/log", 
                period: new TimeSpan(0, 0, 15),
                textFormatter: new NormalTextFormatter(),
                restrictedToMinimumLevel: minimumSeverity,
                httpClient: new AccelistLoggingClient(apiKey)
                );
        }
    }
}
