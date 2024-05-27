using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using backend_teamwork.EntityFramework;

[Table("orders")]
public class Order
{
  [Column("order_id")]
  public Guid OrderId { get; set; }

  [Column("date")]
  public DateTime Date { get; set; } = DateTime.UtcNow;

  [Range(1, double.MaxValue, ErrorMessage = "Total price must be greater than 0")]
  [Column("total_price")]
  public decimal TotalPrice { get; set; }

  [Required]
  [Column("status")]
  public string OrderStatus { get; set; } = "pending"; 

  [Column("user_id")]
  public Guid UserId { get; set; }

  // Relations
  public User User { get; set; } // 1-M with Users
  public List<OrderProduct> OrderProducts { get; set; } = new(); // M-M with Products
}
