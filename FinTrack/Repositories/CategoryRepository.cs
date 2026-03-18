using Dapper;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace FinTrack.Repositories
{
    public class CategoryRepository(SqlConnection connection)
    {
        public async Task<List<Category>> GetCategories(int user)
        {
            string sql = "SELECT * FROM Categories c WHERE (c.UserId = @UserId OR c.IsDefault = 1)";
            IEnumerable<Models.Category>? categories = await connection.QueryAsync<Models.Category>(sql, param: new { UserId = user });
            return categories.ToList();
        }

        public async Task CreateCategory(int user, CreateCategoryDto category)
        {
            string sql = "INSERT INTO Categories (UserId, Name, IsDefault) VALUES (@UserId, @Name, @IsDefault)";
            await connection.QueryAsync(sql, param: new { UserId = user, category.Name, IsDefault = 0 });
        }
    }
}
