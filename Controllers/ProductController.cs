using backend_teamwork.DTOs;
using backend_teamwork.EntityFramework;
using backend_teamwork.Models;
using backend_teamwork.Services;
using backend_teamwork1.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace backend_teamwork.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(AppDbContext appDbContext)
        {
            _productService = new ProductService(appDbContext);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllProducts(
    [FromQuery] string? searchTerm,
    [FromQuery] string? sortBy,
    [FromQuery] string? sortOrder,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 3,
    [FromQuery] List<Guid>? selectedCategories = null,
    [FromQuery] decimal? minPrice = null,
    [FromQuery] decimal? maxPrice = null)
        {
            try
            {
                var queryParameters = new QueryParameters
                {
                    SearchTerm = searchTerm,
                    SortBy = sortBy,
                    SortOrder = sortOrder,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SelectedCategories = selectedCategories,
                    MinPrice = minPrice,
                    MaxPrice = maxPrice
                };

                var products = await _productService.GetAllProducts(queryParameters);
                if (products.Items == null || !products.Items.Any())
                    return NotFound("There are no Products");
                else
                    return Ok(products.Items);
            }
            catch (Exception e)
            {
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }



        [AllowAnonymous]
        [HttpGet("{keyword}")]
        public async Task<IActionResult> SearchForProducts(string keyword)
        {
            try
            {
                var products = await _productService.SearchForProduct(keyword);
                if (products.Count() < 1)
                    return ApiResponse.NotFound("There are no Products Match !!");
                else
                    return ApiResponse.Success(products, "The Products are returned Successfully");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet("get/{identifier}", Name = "GetProductById")]
        public async Task<IActionResult> GetProductById(string identifier)
        {
            try
            {

                if (Guid.TryParse(identifier, out Guid productId))
                {
                    var productById = await _productService.GetProductById(productId);
                    if (productById != null)
                        return ApiResponse.Success(productById, "Product found by ID");
                }

                // If not a valid Guid, assume it's a slug and try to find by slug
                var productBySlug = await _productService.GetProductBySlug(identifier);
                if (productBySlug != null)
                    return ApiResponse.Success(productBySlug, "Product found by slug");

             
                return ApiResponse.NotFound("Product not found");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct(CreateProductDto newProduct)
        {
            try
            {
                var product = await _productService.AddProduct(newProduct);
                return ApiResponse.Created(product, "The product is added successfully");
            }
            catch (DbUpdateException e) when (e.InnerException is Npgsql.PostgresException post)
            {
                if (post.SqlState == "23505")
                    return ApiResponse.Conflict("Duplicate Product");
                else
                    return StatusCode(500, "Custom error for now");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProductById(Guid productId, CreateProductDto newProduct)
        {
            try
            {
                var product = await _productService.UpdateProductById(productId, newProduct);
                if (product == null)
                    return ApiResponse.NotFound("The Product Not found");
                else
                    return ApiResponse.Success(product, "The Product is Updated Successfully");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

        [HttpDelete("{productId}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProductById(Guid productId)
        {
            try
            {
                var result = await _productService.DeleteProduct(productId);
                if (result == false)
                    return ApiResponse.NotFound("The Product Not found");
                else
                    return ApiResponse.Success("The Product is Deleted Successfully");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }
    }
}
