using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace System.Net.Http
{
    /// <summary>
    /// Provides HTTP content based on a UTF-8 JSON-serialized object.
    /// </summary>
    public class JsonContent : StringContent
    {
        /// <summary>
        /// Creates a new instance of JsonContent class using default encoding (UTF-8)
        /// </summary>
        /// <param name="payload"></param>
        public JsonContent(object payload) : base(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json")
        {
        }

        /// <summary>
        /// Creates a new instance of JsonContent class with specified encoding.
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="encoding"></param>
        public JsonContent(object payload, Encoding encoding) : base(JsonConvert.SerializeObject(payload), encoding, "application/json")
        {
        }
    }
}
