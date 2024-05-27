using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.EntityFramework;

namespace backend_teamwork.Models
{
    [Table("category")] 
    public class Category
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

        [Required]
        [Column("createdat")] 
        public DateTime CreatedAt { get; set; }

        public List<Product> Products { get; set; }
    }
}