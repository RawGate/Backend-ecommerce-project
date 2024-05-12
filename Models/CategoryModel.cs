using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.EntityFramework;

namespace backend_teamwork.Models
{
    public class CategoryModel
    {
        [Key]
        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Required]
        [Column("name")]
        [MaxLength(100)]
        public string Name { get; set; } 

        [Required]
        [Column("slug")]
        [MaxLength(100)]
        //[Index(IsUnique = true)]
        public string Slug { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Required]
        [Column("createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}
