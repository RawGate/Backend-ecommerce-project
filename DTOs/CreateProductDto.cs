using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace backend_teamwork.DTOs
{
    public class CreateProductDto
    {
        [MaxLength(100, ErrorMessage = "the name of product must less than 100 character")]
        [MinLength(2, ErrorMessage = "the name of product must more than 2 character")]
        [Required(ErrorMessage = "Product name is required")]
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Product price is required")]
        [Range(0, int.MaxValue, ErrorMessage = $"the Price can not be negative number")]
        public decimal Price { get; set; } = 0;

        [Required(ErrorMessage = "Product Stock is required")]
        [Range(0, int.MaxValue, ErrorMessage = $"the Stock can not be negative number")]
        public int Stock { get; set; } = 0;

        [Required(ErrorMessage = "The  categoryId of the product is required")]
        public Guid CategoryId { get; set; }

    }
}