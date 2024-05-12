using System.ComponentModel.DataAnnotations;

namespace backend_teamwork1.DTOs
{
  public class NewOrderDto
  {
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Total price is required")]
    [Range(1, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
    public decimal TotalPrice { get; set; }
    public List<NewOrderProduct> Products { get; set; } = new List<NewOrderProduct>();
  }

  public class NewOrderProduct
  {
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }
  }
}

