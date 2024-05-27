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
using backend_teamwork.Helpers;

namespace backend_teamwork.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class CategoryController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly ICategoryService _categoryService;

        public CategoryController(AppDbContext appDbContext, ICategoryService categoryService)
        {
            _appDbContext = appDbContext;
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string searchTerm = "", [FromQuery] string sortBy = "")
        {
            try
            {
                var query = _appDbContext.Categories
                    .Include(c => c.Products)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(c => c.Name.Contains(searchTerm));
                }

                if (sortBy.ToLower() == "productcount")
                {
                    query = query.OrderByDescending(c => c.Products.Count);
                }
                else
                {
                    query = query.OrderBy(c => c.Name);
                }

                var categories = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var categoryDtos = categories.Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Slug = c.Slug,
                    Description = c.Description,
                    CreatedAt = c.CreatedAt,
                    Products = c.Products.ToList()
                }).ToList();

                return Ok(categoryDtos);
            }
            catch (Exception ex)
            {
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
                //Products = category.Products.ToList()
            };

            return categoryDto;
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<CategoryDto>> PostCategory(CreateCategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var category = new Category
                {
                    CategoryId = Guid.NewGuid(),
                    Name = categoryDto.Name,
                    Slug = Helper.GenerateSlug(categoryDto.Name),
                    Description = categoryDto.Description,
                    CreatedAt = DateTime.UtcNow
                };

                _appDbContext.Categories.Add(category);
                await _appDbContext.SaveChangesAsync();

                var categoryResponse = new CategoryDto
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name,
                    Slug = category.Slug,
                    Description = category.Description,
                    CreatedAt = category.CreatedAt
                };

                return CreatedAtAction(nameof(GetCategory), new { id = categoryResponse.CategoryId }, categoryResponse);
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine(ex.ToString());

                if (ex.InnerException != null)
                {
                    return StatusCode(500, $"An error occurred while creating the category: {ex.InnerException.Message}");
                }

                return StatusCode(500, $"An error occurred while creating the category: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"An error occurred while creating the category: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] CreateCategoryDto dto)
        {
            try
            {
                Console.WriteLine($"Received request to update category with ID: {id}");
                var result = await _categoryService.UpdateCategory(id, dto); // Correct method call
                if (result)
                {
                    Console.WriteLine("Category update successful");
                    return Ok();
                }
                else
                {
                    Console.WriteLine($"Category with ID {id} not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
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
                return StatusCode(500, $"An error occurred while deleting the category: {ex.Message}");
            }
        }

        private bool CategoryExists(Guid id)
        {
            return _appDbContext.Categories.Any(e => e.CategoryId == id);
        }
    }
}