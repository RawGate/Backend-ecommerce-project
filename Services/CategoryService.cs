using backend_teamwork1.DTOs;
using backend_teamwork1.Services;
using backend_teamwork.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using backend_teamwork.Models;
using backend_teamwork.Helpers;

namespace backend_teamwork.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _appDbContext;

        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories(int pageNumber, int pageSize)
        
        {
            try
            {
                
                var categories = await _appDbContext.Categories
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Slug = c.Slug
                    })
                    .ToListAsync();

                return categories;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while getting the categories. " + e.Message);
            }
        }

        public async Task<Category> AddCategory(CreateCategoryDto newCategory)
        {
            try
            {
                Category category = new Category
                {
                    Name = newCategory.Name,
                    Slug = Helper.GenerateSlug(newCategory.Name),
                    Description = newCategory.Description,
                    CreatedAt = DateTime.UtcNow
                };
                await _appDbContext.Categories.AddAsync(category);
                await _appDbContext.SaveChangesAsync();

                return category;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while adding the category. " + e.Message);
            }
        }

        public async Task<CategoryDto> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _appDbContext.Categories
                    .Include(c => c.Products)
                    .Where(c => c.CategoryId == categoryId)
                    .Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Slug = c.Slug,
                        Description = c.Description,
                        CreatedAt = c.CreatedAt,
                        Products = c.Products
                    })
                    .FirstOrDefaultAsync();

                return category;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while getting the category. " + e.Message);
            }
        }
        public async Task<bool> UpdateCategory(Guid categoryId, CreateCategoryDto dto)
        {
            try
            {
                var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                if (category != null)
                {
                    category.Name = dto.Name;
                    category.Description = dto.Description;

                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while updating the category. " + e.Message);
            }
        }

        public async Task<bool> DeleteCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                if (category != null)
                {
                    _appDbContext.Categories.Remove(category);
                    await _appDbContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new InvalidOperationException("Category not found");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while deleting the category. " + e.Message);
            }
        }
    }
}
public interface ICategoryService
{
    Task<bool> UpdateCategory(Guid categoryId, CreateCategoryDto dto); 
}
