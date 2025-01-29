using AuthenticationAndAuthorization.Data;
using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;
using AuthenticationAndAuthorization.Services;
using Microsoft.Extensions.Configuration;

namespace AuthenticationAndAuthorization.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly AppDbContext _context;


        public AuthRepository(IUserRepository userRepository, ITokenService tokenService, AppDbContext context)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _context = context;
        }

        public async Task<TokenModel> Login(LoginCommand login)
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

            var acessToken = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(60);
            user.RefreshToken = refreshToken;

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AcessToken = acessToken,
                RefreshToken = refreshToken
            };
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

        public async Task<TokenModel> GenerateNewAcessToken(TokenModel model, string email)
        {
            string acessToken = model.AcessToken;
            string refreshToken = model.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(acessToken);

            if (principal is null)
                throw new ArgumentException("Invalid access/refresh token");

            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || user.RefreshToken != refreshToken
            || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                throw new ArgumentException("Invalid access/refresh token");
            }

            var newAccessToken = _tokenService.GenerateToken(user);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            _context.User.Update(user);
            await _context.SaveChangesAsync();

            return new TokenModel
            {
                AcessToken = acessToken,
                RefreshToken = refreshToken,
            };


        }
    }
}
