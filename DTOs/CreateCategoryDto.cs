using System.ComponentModel.DataAnnotations;

namespace backend_teamwork1.DTOs
{
    public class CreateCategoryDto
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
