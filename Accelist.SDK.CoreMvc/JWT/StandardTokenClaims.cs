using System;
using System.Collections.Generic;
using System.Security.Claims;
using Newtonsoft.Json;

namespace Accelist.SDK.CoreMvc.JWT
{
    public class StandardTokenClaims
    {
        [JsonProperty("iss")]
        public string Issuer { set; get; }

        [JsonProperty("aud")]
        public string Audience { set; get; }

        public string Name { set; get; }

        [JsonProperty("sub")]
        public string Username { set; get; }

        public string Email { set; get; }

        [JsonProperty("jti")]
        public Guid TokenId { set; get; }

        public DateTimeOffset Expiration { set; get; }

        [JsonProperty("exp")]
        public long ExpirationUnix
        {
            get
            {
                return Expiration.ToUnixTimeSeconds();
            }
        }

        public DateTimeOffset IssuedAt { set; get; }

        [JsonProperty("iat")]
        public long IssuedAtUnix
        {
            get
            {
                return IssuedAt.ToUnixTimeSeconds();
            }
        }

        public List<string> Roles { set; get; }

        public virtual ClaimsPrincipal ToClaimsPrincipal(string authenticationScheme)
        {
            // https://www.iana.org/assignments/jwt/jwt.xhtml
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, this.Name));
            claims.Add(new Claim(ClaimTypes.Email, this.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, this.Username));
            claims.Add(new Claim(ClaimTypes.Expiration, this.Expiration.ToUniversalTime().ToString()));

            foreach (var role in this.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var id = new ClaimsIdentity(claims, authenticationScheme);
            var principal = new ClaimsPrincipal(id);

            return principal;
        }
    }
}
