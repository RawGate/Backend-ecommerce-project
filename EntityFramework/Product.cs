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
        [Column("product_id")] 
        public Guid ProductId { get; set; }

        [MaxLength(100, ErrorMessage = "The product name must be less than 10 characters.")]
        [MinLength(2, ErrorMessage = "The product name must be more than 2 characters.")]
        [Required(ErrorMessage = "Product name is required.")]
        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("slug")] 
        public string Slug { get; set; } = string.Empty;

        [Column("image")] 
        public string Image { get; set; } = string.Empty;

        [Column("description")] 
        public string Description { get; set; } = string.Empty;

        /*[MaxLength(10, ErrorMessage = "The product color must be less than 10 characters.")]
        [MinLength(2, ErrorMessage = "The product color must be more than 2 characters.")]
        [Required(ErrorMessage = "Product color is required.")]
        [Column("color")] // Maps to the 'color' column in the database
        public string Color { get; set; } = string.Empty;*/

        [Range(0, int.MaxValue, ErrorMessage = "The sold quantity cannot be negative.")]
        [Column("sold_quantity")] 
        public int SoldQuantity { get; set; } = 0;

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The price cannot be negative.")]
        [Column("price")] 
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "Product stock is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "The stock cannot be negative.")]
        [Column("stock")] 
        public int Stock { get; set; } = 0;

        [Column("category_id")] 
        public Guid CategoryId { get; set; }
        public List<Category> Categories { get; set; }

        
        public Category Category { get; set; }

        [Column("createdat")] 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

     
        public List<OrderProduct> OrderProducts { get; set; }
    }
}

