
namespace backend_teamwork1.DTOs
{
  public class OrderDto
  {
    public Guid OrderId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
    public decimal TotalPrice { get; set; }
    public string OrderStatus { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
  }
}
