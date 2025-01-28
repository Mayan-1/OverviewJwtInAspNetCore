using AuthenticationAndAuthorization.DTOs;
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
        public async Task<ActionResult<string>> Login(LoginCommand login)
        {
            var response = await _authRepository.Login(login);

            return Ok(response);
        }
    }
}
