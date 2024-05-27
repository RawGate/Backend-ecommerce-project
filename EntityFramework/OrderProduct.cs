using System.ComponentModel.DataAnnotations.Schema;

namespace backend_teamwork.EntityFramework
{
  [Table("order_product")]
  public class OrderProduct
  {
    [Column("order_product_id")]
    public Guid OrderProductId { get; set; }
    [Column("order_id")]
    public Guid OrderId { get; set; }
    [Column("product_id")]
    public Guid ProductId { get; set; }
    [Column("product_quantity")]
    public int ProductQuantity { get; set; }

    //Relations
    public Order? Order { get; set; }
    public Product? Product { get; set; }
  }
}
