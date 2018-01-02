using System;
using Jose;
using Microsoft.AspNetCore.Authentication;

namespace Accelist.SDK.CoreMvc.JWT
{
    /// <summary>
    /// Contains values required to validate a JWT authentication.
    /// </summary>
    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
        /// <summary>
        /// Sets or gets the simple string describing who the token was intended for.
        /// </summary>
        public string Audience { set; get; }

        /// <summary>
        /// Sets or gets the secret key required for generating the token signature.
        /// </summary>
        public object SigningKey { set; get; }

        /// <summary>
        /// Sets or gets the selected algorithm for generating the token signature.
        /// </summary>
        public JwsAlgorithm SigningAlgorithm { set; get; }
    }
}
