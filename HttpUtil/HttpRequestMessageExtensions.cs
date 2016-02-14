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
    /// Contains extension methods for HttpRequestMessage object.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Throws a HttpResponseException using input status code and message.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="statusCode"></param>
        /// <param name="message"></param>
        public static void ThrowHttpResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string message)
        {
            var response = request.CreateErrorResponse(statusCode, message);
            throw new HttpResponseException(response);
        }
    }
}
