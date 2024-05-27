using backend_teamwork.DTOs;
using backend_teamwork.EntityFramework;

public class CategoryDto
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Product> Products { get; set; } // Ensure the ProductDto is correctly defined
}
