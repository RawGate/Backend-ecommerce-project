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
    [Route("/api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(AppDbContext appDbContext)
        {
            _productService = new ProductService(appDbContext);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] string? filteringTerm, [FromQuery] string? sortColumn, [FromQuery] string? sortOrder, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            try
            {
                var products = await _productService.GetAllProducts(filteringTerm, sortColumn, sortOrder, pageNumber, pageSize);
                if (products.TotalCount < 1)
                    return ApiResponse.NotFound("There are no Products");
                else
                    return ApiResponse.Success(products, "The Products are returned Successfully");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

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

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(Guid productId)
        {
            try
            {
                var product = await _productService.GetProductById(productId);
                if (product == null)
                    return ApiResponse.NotFound("The Product Not found");
                else
                    return ApiResponse.Success(product, "The Product is returned Successfully ");
            }
            catch (Exception e)
            {
                return ApiResponse.ServerError(e.Message);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")] // Requires "Admin" role
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
        [Authorize(Roles = "Admin")] // Requires "Admin" role
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
        [Authorize(Roles = "Admin")] // Requires "Admin" role
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
