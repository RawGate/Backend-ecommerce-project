using backend_teamwork.EntityFramework;
using backend_teamwork.Models;
using backend_teamwork1.DTOs;
using Microsoft.EntityFrameworkCore;

namespace backend_teamwork.Services
{
  public class OrderService
  {
    private readonly AppDbContext _appDbContext;

    public OrderService(AppDbContext appDbContext)
    {
      _appDbContext = appDbContext;
    }

    // Get all orders with selected fields
    public async Task<List<OrderDto>> GetAllOrdersService()
    {
      var order = await _appDbContext.Orders
      .Include(_ => _.User)
      .Select(order => new OrderDto
      {
        OrderId = order.OrderId,
        Date = order.Date,
        OrderStatus = order.OrderStatus,
        UserId = order.UserId,
        UserName = order.User.Name
      }).ToListAsync();

      return order;
    }

    // Get all orders by user id (logged in user)
    public async Task<List<Order>> GetUserOrdersService(Guid userId)
    {
      var userOrders = await _appDbContext.Orders
      .Include(_ => _.OrderProducts)
       .ThenInclude(_ => _.Product)
     .Where(order => order.UserId == userId).ToListAsync();

      return userOrders;
    }

    // Get order by order id
    public async Task<Order?> GetOrderByIdService(Guid orderId)
    {
      return await _appDbContext.Orders
      .Include(_ => _.OrderProducts)
       .ThenInclude(_ => _.Product)
      .Include(_ => _.User)
      .FirstOrDefaultAsync(order => order.OrderId == orderId);
    }

    // Add Order
    public async Task<Order> AddOrderService(NewOrderDto newOrder)
    {
      try
      {
        Order order = new()
        {
          OrderId = Guid.NewGuid(),
          Date = DateTime.UtcNow,
          TotalPrice = newOrder.TotalPrice,
          UserId = newOrder.UserId
        };

        foreach (var product in newOrder.Products)
        {
          order.OrderProducts.Add(new OrderProduct
          {
            OrderProductId = Guid.NewGuid(),
            ProductId = product.ProductId,
            ProductQuantity = product.ProductQuantity,
            OrderId = order.OrderId
          });
        }

        await _appDbContext.Orders.AddAsync(order);
        await _appDbContext.SaveChangesAsync();

        return order;
      }
      catch (DbUpdateException e)
      {
        throw new InvalidOperationException("Could not save the order to database: ", e);
      }
    }

    // Update order by id
    public async Task<Order?> UpdateOrderService(Guid orderId, UpdateOrderDto updateOrder)
    {
      var order = await _appDbContext.Orders.FirstOrDefaultAsync(order => order.OrderId == orderId);
      if (order != null)
      {
        order.OrderStatus = updateOrder.OrderStatusUpdate;
        await _appDbContext.SaveChangesAsync();
      }
      return order;
    }

    // Delete order by id
    public async Task DeleteOrderService(Guid orderId)
    {
      var order = _appDbContext.Orders.FirstOrDefault(order => order.OrderId == orderId);
      if (order != null)
      {
        _appDbContext.Orders.Remove(order);
        await _appDbContext.SaveChangesAsync();
      }
    }
  }
}