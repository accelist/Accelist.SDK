using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accelist.WebUtilities.Logging;
using Serilog.Configuration;
using Serilog.Events;

namespace Serilog
{
    public static class AccelistLoggingExtensions
    {
        public static LoggerConfiguration Accelist(this LoggerSinkConfiguration sinkConfiguration, Guid appId, string server = "https://log.accelist.com", LogEventLevel minimumSeverity = LogEventLevel.Information)
        {
            var url = server.TrimEnd('/') + "/api/v1/log";
            return sinkConfiguration.Http(url, new Sinks.Http.Options(), httpClient: new AccelistLogClient(appId), restrictedToMinimumLevel: minimumSeverity);
        }
    }
}
