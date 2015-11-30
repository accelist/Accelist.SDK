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
    public static class HttpResponseExceptionFactory
    {
        /// <summary>
        /// Throws new HttpResponseException with provided status code and message.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public static void ThrowNew(HttpStatusCode code, string message)
        {
            var body = JsonConvert.SerializeObject(new
            {
                Message = message
            });
            var response = new HttpResponseMessage(code)
            {
                Content = new StringContent(body)
            };
            throw new HttpResponseException(response);
        }
    }
}
