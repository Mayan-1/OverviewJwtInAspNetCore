using AuthenticationAndAuthorization.Data;
using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationAndAuthorization.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Create(UserCreateDto userDto)
        {
            var encryptedPassword = EncryptPassword(userDto.Password);

            var user = new User
            {
                Email = userDto.Email,
                Password = encryptedPassword,
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return "Usuário criado com sucesso";
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                return null;
            }

            return user;
        }

        public string EncryptPassword(string password)
        {
            MD5 mD5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = mD5.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}
