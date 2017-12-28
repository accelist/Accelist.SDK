using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jose;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Accelist.SDK.CoreMvc
{
    public class JoseNewtonsoftMapper : IJsonMapper
    {
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

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None, CamelCaseSerializerSetting);
        }

        public T Parse<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
