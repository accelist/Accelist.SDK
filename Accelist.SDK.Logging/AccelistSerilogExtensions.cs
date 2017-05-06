using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.SDK.Logging;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog
{
    public static class AccelistSerilogExtensions
    {
        public static LoggerConfiguration Accelist(this LoggerSinkConfiguration sinkConfiguration, Guid appId, string server = "https://log.accelist.com", LogEventLevel minimumSeverity = LogEventLevel.Information)
        {
            var url = server.TrimEnd('/') + "/api/v1/log";
            var options = new Sinks.Http.DurableOptions
            {
                FormattingType = Sinks.Http.FormattingType.Normal,
                Period = new TimeSpan(0, 0, 15)
            };
            var client = new AccelistLoggingClient(appId);
            return sinkConfiguration.DurableHttp(url, options, httpClient: client, restrictedToMinimumLevel: minimumSeverity);
        }
    }
}
