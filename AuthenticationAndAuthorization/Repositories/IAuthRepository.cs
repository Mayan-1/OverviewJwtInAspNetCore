using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;

namespace AuthenticationAndAuthorization.Repositories
{
    public interface IAuthRepository
    {
        Task<TokenModel> Login(LoginCommand login);
        bool ComparePassword(User user, string password);
        Task<TokenModel> GenerateNewAcessToken(TokenModel model, string email);
    }
}
