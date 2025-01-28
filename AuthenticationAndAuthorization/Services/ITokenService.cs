using AuthenticationAndAuthorization.Models;

namespace AuthenticationAndAuthorization.Services;

public interface ITokenService
{
    public string GenerateToken(User user);
}
