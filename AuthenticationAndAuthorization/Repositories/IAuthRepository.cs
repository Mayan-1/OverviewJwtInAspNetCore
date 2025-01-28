using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;

namespace AuthenticationAndAuthorization.Repositories
{
    public interface IAuthRepository
    {
        Task<string> Login(LoginCommand login);
        bool ComparePassword(User user, string password);
    }
}
