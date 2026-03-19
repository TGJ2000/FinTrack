using FinTrack.DTOs;
using FinTrack.Models;
using FinTrack.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IConfiguration config, UserRepository userRepo) : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto registerUser)
        {
            registerUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            await userRepo.CreateUser(registerUser);
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser([FromBody] LoginDto loginUser)
        {
            User user = await userRepo.GetUserByEmail(loginUser);

            if (user == null)
            {
                return Unauthorized();
            }
            if (BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
            {
                string jwtKey = config["Jwt:key"];

                SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(jwtKey));

                SigningCredentials signingCredentials = new(key, SecurityAlgorithms.HmacSha256);

                List<Claim> claims = new()
                {
                    new Claim(type: ClaimTypes.NameIdentifier, user.UserId.ToString())
                };

                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: config["Jwt:Issuer"],
                    audience: config["Jwt:Audience"],
                    claims: claims,
                    signingCredentials: signingCredentials,
                    expires: DateTime.Now.AddMinutes(60)
                    );

                string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
