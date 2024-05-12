using System.ComponentModel.DataAnnotations;

namespace backend_teamwork1.DTOs
{
  public class UpdateOrderDto
  {
    public Guid UserId { get; set; }

    [Required(ErrorMessage = "Order Status is required")]
    public string OrderStatusUpdate { get; set; } = "Pending";
  }
}
