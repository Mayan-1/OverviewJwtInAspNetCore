using AuthenticationAndAuthorization.DTOs;
using AuthenticationAndAuthorization.Models;
using AuthenticationAndAuthorization.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationAndAuthorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Create(UserCreateDto userDto)
        {
            var response = await _userRepository.Create(userDto);
            return Ok("User created");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<User>> GetByEmail(string email)
        {
            var response = await _userRepository.GetByEmailAsync(email);
            if (response == null)
            {
                return NotFound("User not found");
            }
            return Ok(response);
        }
    }
}
