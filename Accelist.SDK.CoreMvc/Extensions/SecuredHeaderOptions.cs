using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Options to be passed into the security header generator middleware.
    /// </summary>
    public class SecuredHeaderOptions
    {
        /// <summary>
        /// Contains domains whose scripts are allowed to be loaded into the application.
        /// </summary>
        public IEnumerable<string> ContentSecurityPolicyWhitelist { set; get; }

        /// <summary>
        /// Generates content-security-policy header value using given secured header options.
        /// </summary>
        /// <returns></returns>
        public string SerializeContentSecurityPolicy()
        {
            var hasWhitelist = this.ContentSecurityPolicyWhitelist?.Any() ?? false;

            var sb = new StringBuilder("script-src 'self'");

            if (hasWhitelist)
            {
                foreach (var s in ContentSecurityPolicyWhitelist)
                {
                    sb.Append(" " + s);
                }
            }

            return sb.ToString();
        }
    }
}
