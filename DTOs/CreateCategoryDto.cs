using System.ComponentModel.DataAnnotations;
using backend_teamwork.EntityFramework;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace backend_teamwork1.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
