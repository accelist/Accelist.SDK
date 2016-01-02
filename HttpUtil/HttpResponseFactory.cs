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
    /// Provides helper methods for creating ASP.NET Web API HTTP Responses.
    /// </summary>
    public static class HttpResponseFactory
    {
        /// <summary>
        /// Creates new HTTP response message using provided HTTP status code and JSON-serialized data.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseMessage CreateNewMessage(HttpStatusCode code, object data)
        {
            var response = new HttpResponseMessage(code);

            if (data != null)
            {
                response.Content = new JsonContent(data);
            }

            return response;
        }

        /// <summary>
        /// Creates new HTTP response exception using provided HTTP status code and JSON-serialized data.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static HttpResponseException CreateNewException(HttpStatusCode code, object data)
        {
            return new HttpResponseException(CreateNewMessage(code, data));
        }

        /// <summary>
        /// If condition resolves to TRUE, throws new HTTP response exception using provided HTTP status code and JSON-serialized data.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="code"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static void AssertThrow(this bool condition, HttpStatusCode code, object data = null)
        {
            if (condition)
            {
                throw CreateNewException(code, data);
            }
        }
    }
}
