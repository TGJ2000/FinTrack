using FinTrack.DTOs;

namespace FinTrack.Services
{
    public interface IUserService
    {
        Task CreateUser(RegisterDto registerUser);
        Task<string?> LoginUser(LoginDto loginUser);
    }
}
