using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;

namespace AuthenticationAndAuthorization.Repositories
{
    public interface IUserRepository
    {
        Task<string> Create(UserCreateDto userDto);
        Task<User> GetByEmailAsync(string email);
        string EncryptPassword(string password);

    }
}
