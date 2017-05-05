using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accelist.WebUtilities.Mvc
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
        public static string Serialize(SecuredHeaderOptions option)
        {
            var hasWhitelist = option?.ContentSecurityPolicyWhitelist?.Any() ?? false;

            var result = "script-src 'self'";
            if (hasWhitelist == false)
            {
                return result;
            }

            var whitelist = string.Join(" ", option.ContentSecurityPolicyWhitelist);
            return result + " " + whitelist;
        }
    }
}
