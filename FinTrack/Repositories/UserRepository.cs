using Dapper;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public class UserRepository(IDbConnectionFactory connectionFactory)
    {
        public async Task CreateUser(RegisterDto user)
        {
            using SqlConnection connection = connectionFactory.CreateConnection();
            DateTime createdAt = DateTime.Now;
            string sql = "INSERT INTO Users(Email, PasswordHash, CreatedAt) VALUES (@Email, @PasswordHash, @CreatedAt)";
            await connection.ExecuteAsync(sql, param: new {user.Email, PasswordHash = user.Password, CreatedAt = createdAt});
        }

        public async Task<User?> GetUserByEmail(LoginDto loginUser)
        {
            using SqlConnection connection = connectionFactory.CreateConnection();
            string sql = "SELECT * FROM Users WHERE Users.Email = @Email";
            User user = await connection.QuerySingleOrDefaultAsync<User>(sql, param: new { loginUser.Email });
            return user;

        }
    }
}
