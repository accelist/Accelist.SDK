using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Accelist.SDK.CoreMvc.JWT
{
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        private (StandardTokenClaims claims, string error) TryParseToken(string token)
        {
            try
            {
                var claims = Jose.JWT.Decode<StandardTokenClaims>(token, this.Options.SigningKey, this.Options.SigningAlgorithm);
                return (claims, null);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var token = TryCaptureTokenFromHeader(Context.Request.Headers);
            if (token == null)
            {
                return AuthenticateResult.Fail("Token was not found!");
            }

            var (claims, error) = TryParseToken(token);
            if (error != null)
            {
                return AuthenticateResult.Fail(error);
            }

            if (claims.Audience != this.Options.ClaimsIssuer)
            {
                return AuthenticateResult.Fail("Token issuer mismatched.");
            }

            if (claims.Audience != this.Options.Audience)
            {
                return AuthenticateResult.Fail("Token audience mismatched.");
            }

            if (Clock.UtcNow > claims.Expiration.AddMinutes(5)) // 5 minutes skew time
            {
                return AuthenticateResult.Fail("Token has expired.");
            }

            var principal = claims.ToClaimsPrincipal(this.Scheme.Name);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, this.Scheme.Name));
        }

        private string TryCaptureTokenFromHeader(IHeaderDictionary headers)
        {
            if (headers.ContainsKey("JWT"))
            {
                return headers["JWT"][0];
            }

            if (headers.ContainsKey("Authorization"))
            {
                var auth = headers["Authorization"][0].Split(' ');
                if (auth[0] == "Bearer")
                {
                    return auth[1];
                }
            }

            return null;
        }
    }
}
