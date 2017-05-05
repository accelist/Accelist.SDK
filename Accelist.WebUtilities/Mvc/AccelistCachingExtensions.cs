using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessagePack;
using MessagePack.Resolvers;

namespace Microsoft.Extensions.Caching.Distributed
{
    /// <summary>
    /// This class contains extension methods for storing random-typed objects to the Core MVC distributed cache.
    /// </summary>
    public static class AccelistCachingExtensions
    {
        /// <summary>
        /// Serialize and compress any object into binary value using MessagePack + LZ4.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] SerializeObject<T>(T value)
        {
            return LZ4MessagePackSerializer.Serialize(value, ContractlessStandardResolver.Instance);
        }

        /// <summary>
        /// Decompress and deserialize a binary into a typed object using MessagePack + LZ4.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(byte[] data)
        {
            if (data == null)
            {
                return default(T);
            }

            return LZ4MessagePackSerializer.Deserialize<T>(data, ContractlessStandardResolver.Instance);
        }

        /// <summary>
        /// Synchronously retrieve an type-casted object from the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObject<T>(this IDistributedCache cache, string key)
        {
            var data = cache.Get(key);
            return DeserializeObject<T>(data);
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
            var data = await cache.GetAsync(key);
            return DeserializeObject<T>(data);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name using the given options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public static void SetObject<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var data = SerializeObject(value);
            cache.Set(key, data, options);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObject<T>(this IDistributedCache cache, string key, T value)
        {
            cache.SetObject(key, value, new DistributedCacheEntryOptions());
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name using the given options.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value, DistributedCacheEntryOptions options)
        {
            var data = SerializeObject(value);
            await cache.SetAsync(key, data, options);
        }

        /// <summary>
        /// Synchronously store an object into the distributed cache with given key name.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cache"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task SetObjectAsync<T>(this IDistributedCache cache, string key, T value)
        {
            await cache.SetObjectAsync(key, value, new DistributedCacheEntryOptions());
        }
    }
}
