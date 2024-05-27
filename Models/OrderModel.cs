using System.ComponentModel.DataAnnotations;

namespace backend_teamwork.Models
{

  public class OrderModel
  {
    [Required]
    public Guid OrderId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required(ErrorMessage = "Total price is required")]
    [Range(1, double.MaxValue, ErrorMessage="Total price must be greater than 0")]
    public decimal TotalPrice { get; set; }

    [Required(ErrorMessage = "Order Status is required")]
    public string OrderStatus { get; set; } = "Pending";


    public Guid UserId { get; set; }

   // public List<ProductModel> Products { get; set; } 
    public List<OrderProductModel> OrderProducts { get; set; } = new List<OrderProductModel>();
  }
}