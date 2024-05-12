using System.ComponentModel.DataAnnotations.Schema;

namespace backend_teamwork.EntityFramework
{
  [Table("OrderProducts")]
  public class OrderProduct
  {
    public Guid OrderProductId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }

    //Relations
    public Order? Order { get; set; }
    public Product? Product { get; set; }
  }
}
