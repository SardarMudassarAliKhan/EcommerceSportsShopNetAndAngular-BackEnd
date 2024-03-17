using Core.Entities.Identity;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser appUser);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
