using FinTrack.DTOs;
using FinTrack.Models;

namespace FinTrack.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetUserByEmail(LoginDto loginUser);
        Task CreateUser(RegisterDto user);
    }
}
