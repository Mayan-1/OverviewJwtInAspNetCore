using AuthenticationAndAuthorization.Data;
using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;
using AuthenticationAndAuthorization.Services;

namespace AuthenticationAndAuthorization.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;


        public AuthRepository(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<string> Login(LoginCommand login)
        {
            var user = await _userRepository.GetByEmailAsync(login.Email);

            if(user is null)
            {
                throw new ArgumentNullException("User not found"); 
            }

            bool validPassword = ComparePassword(user, login.Password);

            if(validPassword is false)
            {
                throw new ArgumentException("Password do not match");
            }

            var token = _tokenService.GenerateToken(user);

            return token;
        }

        public bool ComparePassword(User user, string password)
        {
            var encryptedPassword = _userRepository.EncryptPassword(password);
            if(encryptedPassword != user.Password)
            {
                return false;
            }

            return true;
            
        }
    }
}
