using FinTrack.DTOs;
using FinTrack.Models;
using FinTrack.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/categories")]
    public class CategoryController(CategoryRepository categoryRepository) : ControllerBase
    {
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            int user = GetUserId();
            List<Category> categories = await categoryRepository.GetCategories(user);
            return Ok(categories);

        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto category)
        {
            int user = GetUserId();
            await categoryRepository.CreateCategory(user, category);
            return StatusCode(201);
        }
    }
}
