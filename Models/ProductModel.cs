using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.Models
{
    public class ProductModel
    {
        [Key]
        [Required]
        public Guid ProductId { get; set; }

        [MaxLength(10, ErrorMessage = "the name of product must less than 10 character")]
        [MinLength(2, ErrorMessage = "the name of product must more than 2 character")]
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        [MaxLength(10, ErrorMessage = "the color of product must less than 10 character")]
        [MinLength(2, ErrorMessage = "the color of product must more than 2 character")]
        [Required(ErrorMessage = "Product color is required")]
        public string Color { get; set; } = string.Empty;

        [Range(0, int.MaxValue, ErrorMessage = $"the SoldQuantity can not be negative number")]
        public int SoldQuantity { get; set; } = 0;

        [Required(ErrorMessage = "Product price is required")]
        [Range(0, int.MaxValue, ErrorMessage = $"the Price can not be negative number")]
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "Product Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = $"the Stock can not be negative number")]
        public int Stock { get; set; } = 0;
        
        [Required(ErrorMessage = "The  categoryId of the product is required")]
        public Guid CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


        //Relation M-M between Orders and Products
        List<OrderModel>? OrdersModel { get; set; }

    }
}
