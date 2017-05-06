using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// This class contains extension methods for storing random-typed objects to Core MVC Session.
    /// </summary>
    public static class AccelistSessionExtensions
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
            var ok = session.TryGetValue(key, out byte[] data);
            if (ok)
            {
                return AccelistCachingExtensions.DeserializeObject<T>(data);
            }
            else
            {
                return default(T);
            }
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
    }
}
