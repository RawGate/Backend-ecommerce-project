using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_teamwork1.DTOs;
using backend_teamwork1.Services;
using backend_teamwork.Services;
using backend_teamwork.EntityFramework;
using backend_teamwork.Models;

namespace backend_teamwork.Controllers
{
    [Route("api/category")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            try
            {
                return await _appDbContext.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred while retrieving categories: {ex.Message}");
            }
        }

      [HttpGet("{id}")]
      [AllowAnonymous]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            var category = await _appDbContext.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                Products = category.Products.ToList()
            };

            return categoryDto;
        }
    

        [HttpPost]
        public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = new Category
                {
                    CategoryId = categoryDto.CategoryId,
                    Name = categoryDto.Name,
                    Slug = categoryDto.Slug,
                    Description = categoryDto.Description,
                    CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc) // Convert DateTime to UTC
                };

                _appDbContext.Categories.Add(category);
                await _appDbContext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetCategory), new { id = category.CategoryId }, category);
            }
            catch (DbUpdateException ex)
            {
                // Log the exception details
                Console.WriteLine(ex.ToString());

                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"An error occurred while creating the category: {ex.InnerException.Message}");
                }

                return StatusCode(500, $"An error occurred while creating the category: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine(ex.ToString());

                return StatusCode(500, $"An error occurred while creating the category: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(Guid id, Category category)
        {
            try
            {
                if (id != category.CategoryId)
                {
                    return BadRequest();
                }

                _appDbContext.Entry(category).State = EntityState.Modified;

                await _appDbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred while updating the category: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            try
            {
                var category = await _appDbContext.Categories.FindAsync(id);
                if (category == null)
                {
                    return NotFound();
                }

                _appDbContext.Categories.Remove(category);
                await _appDbContext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as required
                return StatusCode(500, $"An error occurred while deleting the category: {ex.Message}");
            }
        }

        private bool CategoryExists(Guid id)
        {
            return _appDbContext.Categories.Any(e => e.CategoryId == id);
        }
    }
}