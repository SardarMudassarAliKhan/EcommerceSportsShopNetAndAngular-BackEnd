using Core.Entities.Identity;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(AppUser appUser);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}