using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.Models;

namespace backend_teamwork.EntityFramework
{
    [Table("product")]
    public class Product
    {
        [Key]
        [Required]
        [Column("product_id")] // Maps to the 'product_id' column in the database
        public Guid ProductId { get; set; }

        [MaxLength(10, ErrorMessage = "The product name must be less than 10 characters.")]
        [MinLength(2, ErrorMessage = "The product name must be more than 2 characters.")]
        [Required(ErrorMessage = "Product name is required.")]
        [Column("name")] // Maps to the 'name' column in the database
        public string Name { get; set; } = string.Empty;

        [Column("slug")] // Maps to the 'slug' column in the database
        public string Slug { get; set; } = string.Empty;

        [Column("image")] // Maps to the 'image' column in the database
        public string Image { get; set; } = string.Empty;

        [Column("description")] // Maps to the 'description' column in the database
        public string Description { get; set; } = string.Empty;

        /*[MaxLength(10, ErrorMessage = "The product color must be less than 10 characters.")]
        [MinLength(2, ErrorMessage = "The product color must be more than 2 characters.")]
        [Required(ErrorMessage = "Product color is required.")]
        [Column("color")] // Maps to the 'color' column in the database
        public string Color { get; set; } = string.Empty;*/

        [Range(0, int.MaxValue, ErrorMessage = "The sold quantity cannot be negative.")]
        [Column("sold_quantity")] // Maps to the 'sold_quantity' column in the database
        public int SoldQuantity { get; set; } = 0;

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The price cannot be negative.")]
        [Column("price")] // Maps to the 'price' column in the database
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "Product stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
        [Column("stock")] // Maps to the 'stock' column in the database
        public int Stock { get; set; } = 0;

        [Column("category_id")] // Maps to the 'category_id' column in the database
        public Guid CategoryId { get; set; }

        // Navigation property for the Category relationship
        public Category Category { get; set; }

        [Column("createdat")] // Maps to the 'created_at' column in the database
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation property for the M-M relationship between Orders and Products
        public List<OrderProduct> OrderProducts { get; set; }
    }
}

