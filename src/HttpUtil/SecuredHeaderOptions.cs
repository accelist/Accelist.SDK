using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HttpUtil
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
    }
}
