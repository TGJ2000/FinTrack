using FinTrack.DTOs;
using FinTrack.Services;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IUserService userService) : BaseController
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerDto)
        {
            await userService.CreateUser(registerDto);
            return StatusCode(201);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginDto)
        {
            string? token = await userService.LoginUser(loginDto);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new { token });
        }

    }
}
