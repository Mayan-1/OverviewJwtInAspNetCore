using AuthenticationAndAuthorization.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAndAuthorization.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var key = _configuration.GetSection("JWT").GetSection("SecretKey").Value ??
            throw new InvalidOperationException("Invalid secret key");

        var privateKey = Encoding.UTF8.GetBytes(key);

        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey),
            SecurityAlgorithms.HmacSha256Signature);

        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email)
        };

        var expires = DateTime.UtcNow.AddMinutes(double.Parse(_configuration.GetSection("JWT").GetSection("TokenValidityInMinutes").Value));
        var audience = _configuration.GetSection("JWT").GetSection("ValidAudience").Value;
        var issuer = _configuration.GetSection("JWT").GetSection("ValidIssuer").Value;


        var token = new JwtSecurityToken(
            issuer,
            audience,
            claims,
            null,
            expires,
            signingCredentials);


        string tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue;


    }
}
