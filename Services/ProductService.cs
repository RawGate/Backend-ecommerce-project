using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using backend_teamwork.DTOs;
using backend_teamwork.EntityFramework;
using backend_teamwork.Helpers;
using backend_teamwork.Models;
using backend_teamwork1.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend_teamwork.Services
{
    public class ProductService
    {
        private readonly AppDbContext _appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<PaginationDto<ProductDto>> GetAllProducts(QueryParameters queryParameters)
        {
            IQueryable<Product> productQuery = _appDbContext.Products.Include(p => p.Category);

            if (!string.IsNullOrWhiteSpace(queryParameters.SearchTerm))
            {
                string searchTerm = queryParameters.SearchTerm.ToLower();
                productQuery = productQuery.Where(p => p.Name.ToLower().Contains(searchTerm));
            }

            if (queryParameters.SelectedCategories != null && queryParameters.SelectedCategories.Any())
            {
                productQuery = productQuery.Where(p => queryParameters.SelectedCategories.Contains(p.CategoryId)); 
            }

            if (queryParameters.MinPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price >= queryParameters.MinPrice.Value);
            }

            if (queryParameters.MaxPrice.HasValue)
            {
                productQuery = productQuery.Where(p => p.Price <= queryParameters.MaxPrice.Value);
            }

            var keySelector = GetSortProperty(queryParameters.SortBy);

            if (queryParameters.SortOrder?.ToLower() == "desc")
            {
                productQuery = productQuery.OrderByDescending(keySelector);
            }
            else
            {
                productQuery = productQuery.OrderBy(keySelector);
            }

            var totalProductCount = await _appDbContext.Products.CountAsync();

            var products = await productQuery
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .Select(p => new ProductDto
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Slug = p.Slug,
                    Image = p.Image,
                    Description = p.Description,
                    SoldQuantity = p.SoldQuantity,
                    Price = p.Price,
                    Stock = p.Stock,
                    CategoryId = p.CategoryId,
                    Category = new CategoryDto
                    {
                        CategoryId = p.Category.CategoryId,
                        Name = p.Category.Name,
                    }
                })
                .ToListAsync();

            return new PaginationDto<ProductDto>
            {
                Items = products,
                TotalCount = totalProductCount,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize
            };
        }



        private static Expression<Func<Product, object>> GetSortProperty(string? sortColumn)
        {
            return sortColumn?.ToLower().Trim()
            switch
            {
                "name" => product => product.Name,
                "price" => product => product.Price,
                _ => product => product.ProductId
            };
        }
        public async Task<IEnumerable<Product>> SearchForProduct(string keyword)
        {
            if (_appDbContext.Products == null)
            {
                throw new InvalidOperationException("there is no Product to search");
            }

            var products = from m in _appDbContext.Products
                           select m;

            if (!String.IsNullOrEmpty(keyword))
            {
                products = products.Where(p => p.Name!.Contains(keyword));
            }

            return await products.ToListAsync();
        }

        public async Task<ProductDto> GetProductById(Guid productId)
        {
            try
            {
                var product = await _appDbContext.Products
                .Where(p => p.ProductId == productId)
                .Select(p => new ProductDto
                {
                    Name = p.Name,
                    Slug = p.Slug,
                    Image = p.Image,
                    Description = p.Description,
                    SoldQuantity = p.SoldQuantity,
                    Price = p.Price,
                    Stock = p.Stock,
                    ProductId = p.ProductId, 
                    CategoryId = p.CategoryId
                }).FirstOrDefaultAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while getting the product. " + e.Message);
            }
        }
        public async Task<ProductDto> GetProductBySlug(string slug)
        {
            try
            {
                var product = await _appDbContext.Products
                    .Where(p => p.Slug == slug)
                    .Select(p => new ProductDto
                    {
                        Name = p.Name,
                        Slug = p.Slug,
                        Image = p.Image,
                        Description = p.Description,
                        SoldQuantity = p.SoldQuantity,
                        Price = p.Price,
                        Stock = p.Stock,
                        ProductId = p.ProductId,
                        CategoryId = p.CategoryId
                    })
                    .FirstOrDefaultAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while getting the product by slug. " + e.Message);
            }
        }


        public async Task<Product> AddProduct(CreateProductDto newProduct)
        {
            try
            {

                Product product = new Product
                {
                    Name = newProduct.Name,
                    Slug = Helper.GenerateSlug(newProduct.Name),
                    Image = newProduct.Image,
                    Description = newProduct.Description,
                    Price = newProduct.Price,
                    Stock = newProduct.Stock,
                    CategoryId = newProduct.CategoryId
                };
                await _appDbContext.Products.AddAsync(product);
                await _appDbContext.SaveChangesAsync();

                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while adding the product. " + e.Message);
            }
        }


        public async Task<Product> UpdateProductById(Guid productId, CreateProductDto newProduct)
        {
            try
            {
                var product = _appDbContext.Products.FirstOrDefault(product => product.ProductId == productId);
                if (product != null)
                {
                    product.Name = newProduct.Name;
                    product.Slug = Helper.GenerateSlug(newProduct.Name);
                    product.Image = newProduct.Image;
                    product.Description = newProduct.Description;
                    product.Price = newProduct.Price;
                    product.Stock = newProduct.Stock;
                    product.CategoryId = newProduct.CategoryId;

                }
                else
                {
                    throw new InvalidOperationException("Product not found");
                }
                await _appDbContext.SaveChangesAsync();
                return product;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while updating product. " + e.Message);
            }
        }

        public async Task<bool> DeleteProduct(Guid productId)
        {
            try
            {
                var product = await _appDbContext.Products.FirstOrDefaultAsync(product => product.ProductId == productId);
                if (product != null)
                {
                    _appDbContext.Remove(product);
                    _appDbContext.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Error occurred while deleting product. " + e.Message);
            }
        }
    }
}