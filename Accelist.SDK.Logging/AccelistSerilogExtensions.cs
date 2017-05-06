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
        public static LoggerConfiguration Accelist(this LoggerSinkConfiguration sinkConfiguration, Guid appId, string server = null, LogEventLevel minimumSeverity = LogEventLevel.Information)
        {
            var dom = server?.TrimEnd('/') ?? "https://log.accelist.com";

            var option = new Serilog.Sinks.Http.Options
            {
                FormattingType = Serilog.Sinks.Http.FormattingType.Normal,
                Period = new TimeSpan(0, 0, 15)
            };

            var client = new AccelistLoggingClient(appId);
            return sinkConfiguration.Http(dom + "/api/v1/log", option, minimumSeverity, client);
        }
    }
}
