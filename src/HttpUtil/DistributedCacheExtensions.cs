using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// This class contains extension methods for storing random-typed objects to the Core MVC distributed cache.
    /// </summary>
    public static class DistributedCacheExtensions
    {
        /// <summary>
        /// Synchronously retrieve an type-casted object from the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObject<T>(this IDistributedCache cache, string key)
        {
            var json = cache.GetString(key);
            if (string.IsNullOrEmpty(json)) return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Asynchronously retrieve an type-casted object from the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static async Task<T> GetObjectAsync<T>(this IDistributedCache cache, string key)
        {
            var json = await cache.GetStringAsync(key);
            if (string.IsNullOrEmpty(json)) return default(T);

            return JsonConvert.DeserializeObject<T>(json);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name using the given options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public static void SetObject<T>(this IDistributedCache session, string key, T value, DistributedCacheEntryOptions options)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json, options);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObject<T>(this IDistributedCache session, string key, T value)
        {
            session.SetObject(key, value, new DistributedCacheEntryOptions());
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name using the given options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task SetObjectAsync<T>(this IDistributedCache session, string key, T value, DistributedCacheEntryOptions options)
        {
            var json = JsonConvert.SerializeObject(value);
            await session.SetStringAsync(key, json, options);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task SetObjectAsync<T>(this IDistributedCache session, string key, T value)
        {
            await session.SetObjectAsync(key, value, new DistributedCacheEntryOptions());
        }
    }
}
