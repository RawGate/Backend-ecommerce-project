public class OrderDto
{
  public Guid OrderId { get; set; }
  public DateTime Date { get; set; }
  public string OrderStatus { get; set; }
  public Guid UserId { get; set; }
  public string UserName { get; set; }
  public decimal TotalPrice { get; set; }
}

