using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.EntityFramework;

namespace backend_teamwork1.DTOs
{
    public class CategoryDto
    {
        [Key]
        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [Required]
        [Column("slug")]
        public string Slug { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("createdat")]
        public DateTime CreatedAt { get; set; }  // Ensure this property is of type DateTime

        public List<Product> Products { get; set; }
    }
}