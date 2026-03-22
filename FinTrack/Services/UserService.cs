using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinTrack.Repositories;

namespace FinTrack.Services
{
    public class UserService(IConfiguration config, IUserRepository userRepo) : IUserService
    {
        public async Task CreateUser(RegisterDto registerUser)
        {
            registerUser.Password = BCrypt.Net.BCrypt.HashPassword(registerUser.Password);
            await userRepo.CreateUser(registerUser);
        }
        public async Task<string?> LoginUser(LoginDto loginUser)
        {
            User user = await userRepo.GetUserByEmail(loginUser);

            if (user == null)
            {
                return null;
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

                return tokenString;
            }
            else
            {
                return null;
            }
        }
    }
}
