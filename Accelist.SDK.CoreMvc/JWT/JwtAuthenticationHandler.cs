using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Accelist.SDK.CoreMvc.JWT
{
    /// <summary>
    /// Implements ASP.NET Core MVC AuthenticationHandler class for bearer token authentication using JOSE-JWT library.
    /// </summary>
    public class JwtAuthenticationHandler : AuthenticationHandler<JwtAuthenticationOptions>
    {
        /// <summary>
        /// Creates an instance of JwtAuthenticationHandler using injected dependencies by ASP.NET Core MVC authentication scheme adder.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        /// <param name="encoder"></param>
        /// <param name="clock"></param>
        public JwtAuthenticationHandler(IOptionsMonitor<JwtAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        /// <summary>
        /// Attempts to decode a token string using JOSE-JWT library. Returns both claims and error object for successful or failed parse.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Asynchronously handles bearer token authentication.
        /// </summary>
        /// <returns></returns>
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

            var id = claims.ToClaimsIdentity(this.Scheme.Name);
            var principal = new ClaimsPrincipal(id);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, this.Scheme.Name));
        }

        /// <summary>
        /// Attempts to extract token string from HTTP header (JWT or Authorization: Bearer).
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
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
