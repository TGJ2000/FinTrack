using FinTrack.DTOs;
using FinTrack.Models;

namespace FinTrack.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories(int user);
        Task CreateCategory(int user, CreateCategoryDto category);
    }
}
