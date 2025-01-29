using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;
using AuthenticationAndAuthorization.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        public async Task<ActionResult<TokenModel>> Login(LoginCommand login)
        {
            var response = await _authRepository.Login(login);

            return Ok(response);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<ActionResult<TokenModel>> RefreshToken(TokenModel model, string email) 
        {
            var response = await _authRepository.GenerateNewAcessToken(model, email);
            return Ok(response);
        }
    }
}
