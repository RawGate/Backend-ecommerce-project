using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend_teamwork.DTOs;
using backend_teamwork.EntityFramework;
using backend_teamwork.Helpers;
using backend_teamwork.Models;
using backend_teamwork1.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend_teamwork.Services
{
    public class CategoryService
    {
        private readonly AppDbContext _appDbContext;

        public CategoryService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            try
            {
                var categories = await _appDbContext.Categories
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

        public async Task<CategoryDto> GetCategoryById(Guid categoryId)
        {
            try
            {
                var category = await _appDbContext.Categories
                    .Include(c => c.Products) // Include the Products navigation property
                    .Where(c => c.CategoryId == categoryId)
                    .Select(c => new CategoryDto
                    {
                        CategoryId = c.CategoryId,
                        Name = c.Name,
                        Slug = c.Slug,
                        Products = c.Products // Assign the Products to the DTO
                    })
                    .FirstOrDefaultAsync();

                return category;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while getting the category. " + e.Message);
            }
        }

        public async Task<Category> AddCategory(CreateCategoryDto newCategory)
        {
            try
            {
                Category category = new Category
                {
                    Name = newCategory.Name,
                    Slug = Helper.GenerateSlug(newCategory.Name)
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

        public async Task<Category> UpdateCategoryById(Guid categoryId, CreateCategoryDto newCategory)
        {
            try
            {
                var category = await _appDbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == categoryId);
                if (category != null)
                {
                    category.Name = newCategory.Name;
                    category.Slug = Helper.GenerateSlug(newCategory.Name);
                }
                else
                {
                    throw new InvalidOperationException("Category not found");
                }

                await _appDbContext.SaveChangesAsync();
                return category;
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