using Jose;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accelist.WebUtilities.Json
{
    /// <summary>
    /// JSON.NET mapper for JOSE-JWT library for serializing object payload from any class instead of using Dictionary of string-to-object.
    /// </summary>
    public class JoseNewtonsoftMapper : IJsonMapper
    {
        /// <summary>
        /// Allows direct conversion of values from a class into JWT claims using JSON.NET SerializeObject method.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.SerializeObject(obj, Formatting.Indented, settings);
        }

        /// <summary>
        /// Allows direct extraction of values from a JWT claims into classes using JSON.NET DeserializeObject method.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Parse<T>(string json)
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            return JsonConvert.DeserializeObject<T>(json, settings);
        }
    }
}
