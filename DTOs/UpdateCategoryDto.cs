using System.ComponentModel.DataAnnotations;

namespace backend_teamwork1.DTOs
{
    public class UpdateCategoryDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
