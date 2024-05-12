using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.EntityFramework;

namespace backend_teamwork.Models
{
    [Table("category")] // Specify the table name in the database
    public class Category
    {
        [Key]
        [Column("category_id")] // Map to the 'category_id' column in the database
        public Guid CategoryId { get; set; }

        [Required]
        [Column("name")] // Map to the 'name' column in the database
        public string Name { get; set; }

        [Required]
        [Column("slug")] // Map to the 'slug' column in the database
        public string Slug { get; set; }

        [Column("description")] // Map to the 'description' column in the database
        public string Description { get; set; }

        [Required]
        [Column("createdat")] // Map to the 'createdat' column in the database
        public DateTime CreatedAt { get; set; }

        public List<Product> Products { get; set; }
    }
}