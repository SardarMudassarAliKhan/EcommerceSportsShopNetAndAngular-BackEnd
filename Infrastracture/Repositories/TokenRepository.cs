using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace Infrastracture.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly JsonWebTokenKeys JsonWebTokenKeys;

        public TokenRepository(JsonWebTokenKeys jsonWebTokenKeys)
        {
            this.JsonWebTokenKeys = jsonWebTokenKeys;
        }

        public string GenerateAccessToken(AppUser appUser)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,appUser.Email),
                new Claim(ClaimTypes.GivenName, appUser.DisplayName)
            };

            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonWebTokenKeys.securityKey));
            
            var creds = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = JsonWebTokenKeys.ValidIssuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, // you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true, // assuming this should always be true for security reasons
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonWebTokenKeys.IssuerSigningKey)),
                ValidateLifetime = false
                //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}