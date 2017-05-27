using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Accelist.SDK.REST
{
    /// <summary>
    /// Implementation of .NET HttpClient that does not suck when used as a Singleton.
    /// (Default HttpClient does not honor DNS changes)
    /// </summary>
    public class UltimateHttpClient
    {
        /// <summary>
        /// Singleton instance of the global custom HttpClient instance.
        /// </summary>
        private static HttpClient _Instance;

        /// <summary>
        /// Lock object for instantiating singleton custom HttpClient instance.
        /// </summary>
        private static object _InstanceLock = new object();

        /// <summary>
        /// Lazy accessor for application-wide custom HttpClient singleton instance, that automatically logs and close the socket used for connection when finished.
        /// </summary>
        public static HttpClient Instance
        {
            get
            {
                /*
                Holy Shit
                https://aspnetmonsters.com/2016/08/2016-08-27-httpclientwrong/
                http://stackoverflow.com/questions/7849884/what-is-limiting-the-of-simultaneous-connections-my-asp-net-application-can-ma
                http://byterot.blogspot.co.id/2016/07/singleton-httpclient-dns.html
                https://weblogs.asp.net/johnbilliris/don-t-forget-to-tune-your-application

                HttpClient is different. Although it implements the IDisposable interface it is actually a shared object. 
                This means that under the covers it is reentrant) and thread safe. 
                Instead of creating a new instance of HttpClient for each execution you should share a single instance of HttpClient for the entire lifetime of the application. 

                HttpClient implements IDisposable only indirectly through HttpMessageHandler and only as a result of in-case not an immediate need.
                If you don't reuse HttpClient, Windows will hold the connection socket for 240 seconds, depriving your server!

                In short, the community agreed that it was 100 % safe, not only not disposing the HttpClient, but also to use it as Singleton.
                The main concern was thread safety when making concurrent HTTP calls -and even official documentations said there is no risk doing that.

                But it turns out there is a serious issue: DNS changes are NOT honoured and HttpClient(through HttpClientHandler) hogs the connections until socket is closed.
                Indefinitely.

                This will set the HTTP’s keep-alive header to false so the socket will be closed after a single request. 

                It turns out this can add roughly extra 35ms (with long tails, i.e amplifying outliers) to each of your HTTP calls
                preventing you to take advantage of benefits of re-using a socket.

                By the way, by default outbound connection via HttpClient is limited to 2. Don't forget to do this in app.config:

                <configuration>
                    <system.net>
                        <connectionManagement>
                            <add address="*" maxconnection="2147483647"/>
                        </connectionManagement>
                    </system.net>
                </configuration>
                 */

                if (_Instance == null)
                {
                    lock (_InstanceLock)
                    {
                        if (_Instance == null)
                        {
                            // Let's log the client perf, because we can.
                            _Instance = new HttpClient(new SerilogHttpClientHandler());
                            _Instance.DefaultRequestHeaders.ConnectionClose = true;
                        }
                    }
                }

                return _Instance;
            }
        }
    }
}
