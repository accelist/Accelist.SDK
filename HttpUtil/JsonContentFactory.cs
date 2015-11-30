using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HttpUtil
{
    public static class JsonContentFactory
    {
        /// <summary>
        /// Creates new HttpContent with provided payload, serialized as JSON string and set to "application/json" content type.
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static HttpContent CreateNew(object payload)
        {
            var content = new StringContent(JsonConvert.SerializeObject(payload));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            return content;
        }
    }
}
