using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpUtil.Mvc;

namespace Microsoft.AspNetCore.Builder
{
    public static class HttpUtilApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseApiExceptionHandler(this IApplicationBuilder app, bool debugMode)
        {
            return app.UseMiddleware<ApiExceptionHandlerMiddleware>(debugMode);
        }
    }
}
