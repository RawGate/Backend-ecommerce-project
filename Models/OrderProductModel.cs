

namespace backend_teamwork.Models
{
  public class OrderProductModel
  {
    public Guid orderProductId { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int ProductQuantity { get; set; }


    //Relations M-M between Orders and Products
    public OrderModel? Order { get; set; }
    public ProductModel? Product { get; set; }
  }
}