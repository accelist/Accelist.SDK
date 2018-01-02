using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Accelist.SDK.CoreMvc.JWT
{
    /// <summary>
    /// Implements camel-cased JSON mapper for JOSE-JWT.
    /// </summary>
    public class JoseNewtonsoftMapper : IJsonMapper
    {
        /// <summary>
        /// Returns a new instance of Newtonsoft JSON setting for serializing JSON with no indentation and camel case.
        /// </summary>
        private JsonSerializerSettings CamelCaseSerializerSetting
        {
            get
            {
                return new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    },
                    Formatting = Formatting.None
                };
            }
        }

        /// <summary>
        /// Serialize an object to JSON claims with no indentation and camel case.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, CamelCaseSerializerSetting);
        }

        /// <summary>
        /// Deserialize claims to object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
