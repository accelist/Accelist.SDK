using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// This class contains extension methods for storing random-typed objects to Core MVC Session.
    /// </summary>
    public static class AccelistHttpExtensions
    {
        /// <summary>
        /// Retrieves an type-casted object from the session store with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObject<T>(this ISession session, string key)
        {
            var data = session.Get(key);
            return AccelistCachingExtensions.DeserializeObject<T>(data);
        }

        /// <summary>
        /// Stores an object into the session store with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObject<T>(this ISession session, string key, T value)
        {
            var data = AccelistCachingExtensions.SerializeObject(value);
            session.Set(key, data);
        }

        /// <summary>
        /// This extension method checks whether a request path starts with "/api"
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsApiContext(this HttpContext context)
        {
            return context.Request.Path.Value.StartsWith("/api", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether current request host is a localhost.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static bool IsLocalhost(this HttpContext context)
        {
            var host = context.Request.Host.Host;
            return (host == "localhost") || (host == "127.0.0.1") || (host == "::1");
        }
    }
}
