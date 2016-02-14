using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace System.Net.Http
{
    /// <summary>
    /// Contains extension methods for HttpRequestMessage object.
    /// </summary>
    public static class HttpUtilRequestMessageExtensions
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
