using AuthenticationAndAuthorization.Models;
using System.Security.Claims;

namespace AuthenticationAndAuthorization.Services;

public interface ITokenService
{
    string GenerateToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}
