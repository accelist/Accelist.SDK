using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace HttpUtil
{
    /// <summary>
    /// Provides helper methods for creating ASP.NET Web API HTTP Responses in UTF-8 JSON format.
    /// </summary>
    public static class JsonResponse
    {
        /// <summary>
        /// Creates new HTTP response message using provided HTTP status code and JSON-serialized data.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateNew(HttpStatusCode code, object data)
        {
            var response = new HttpResponseMessage(code);
            response.Content = new JsonContent(data);

            return response;
        }

        /// <summary>
        /// Creates new HTTP response exception using provided HTTP status code and JSON-serialized data.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseException ThrowNew(HttpStatusCode code, object data = null)
        {
            throw new HttpResponseException(CreateNew(code, data));
        }
    }
}
