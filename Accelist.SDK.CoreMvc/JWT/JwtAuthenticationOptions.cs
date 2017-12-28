using Jose;
using Microsoft.AspNetCore.Authentication;

namespace Accelist.SDK.CoreMvc.JWT
{
    public class JwtAuthenticationOptions : AuthenticationSchemeOptions
    {
        public string Audience { set; get; }

        public object SigningKey { set; get; }

        public JwsAlgorithm SigningAlgorithm { set; get; }
    }
}
