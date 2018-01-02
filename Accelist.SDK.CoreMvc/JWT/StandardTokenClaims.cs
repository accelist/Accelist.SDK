using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Accelist.SDK.CoreMvc.JWT
{
    /// <summary>
    /// Standard JWT claims.
    /// </summary>
    public class StandardTokenClaims
    {
        /// <summary>
        /// Sets or gets a simple string describing who generated the token.
        /// </summary>
        [JsonProperty("iss")]
        public string Issuer { set; get; }

        /// <summary>
        /// Sets or gets a simple string describing who the token is intended for.
        /// </summary>
        [JsonProperty("aud")]
        public string Audience { set; get; }

        /// <summary>
        /// Sets or gets the user's full name.
        /// </summary>
        public string Name { set; get; }

        /// <summary>
        /// Sets or gets the user ID or login username.
        /// </summary>
        [JsonProperty("sub")]
        public string Username { set; get; }

        /// <summary>
        /// Sets or gets the user's email.
        /// </summary>
        public string Email { set; get; }

        /// <summary>
        /// Sets or gets the unique token ID or session ID.
        /// </summary>
        [JsonProperty("jti")]
        public Guid TokenId { set; get; }

        /// <summary>
        /// Sets or gets the token expiration time from a ISO 8601 formatted string.
        /// </summary>
        public DateTimeOffset Expiration { set; get; }

        /// <summary>
        /// Gets the token expiration time in UNIX timestamp format.
        /// </summary>
        [JsonProperty("exp")]
        public long ExpirationUnix
        {
            get
            {
                return Expiration.ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// Sets or gets the token issuance time from a ISO 8601 formatted string.
        /// </summary>
        public DateTimeOffset IssuedAt { set; get; }

        /// <summary>
        /// Gets the token issuance time in UNIX timestamp format.
        /// </summary>
        [JsonProperty("iat")]
        public long IssuedAtUnix
        {
            get
            {
                return IssuedAt.ToUnixTimeSeconds();
            }
        }

        /// <summary>
        /// Sets or gets the user application roles.
        /// </summary>
        public List<string> Roles { set; get; }

        /// <summary>
        /// Converts the claims into a ClaimsIdentity object for authentication and authorization using ASP.NET Core MVC middleware.
        /// </summary>
        /// <param name="authenticationScheme"></param>
        /// <returns></returns>
        public virtual ClaimsIdentity ToClaimsIdentity(string authenticationScheme)
        {
            // https://www.iana.org/assignments/jwt/jwt.xhtml
            var id = new ClaimsIdentity(authenticationScheme);

            id.AddClaim(new Claim(ClaimTypes.Name, this.Name));
            id.AddClaim(new Claim(ClaimTypes.Email, this.Email));
            id.AddClaim(new Claim(ClaimTypes.NameIdentifier, this.Username));
            id.AddClaim(new Claim(ClaimTypes.Expiration, this.Expiration.ToUniversalTime().ToString()));

            foreach (var role in this.Roles)
            {
                id.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return id;
        }
    }
}
