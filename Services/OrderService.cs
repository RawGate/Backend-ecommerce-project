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
          UserName = order.User.Name,
          TotalPrice = order.TotalPrice 
        }).ToListAsync();

      return order;
    }


    public async Task<List<Order>> GetUserOrdersService(Guid userId)
    {
      var userOrders = await _appDbContext.Orders
      .Include(_ => _.OrderProducts)
       .ThenInclude(_ => _.Product)
     .Where(order => order.UserId == userId).ToListAsync();

      return userOrders;
    }


    public async Task<Order?> GetOrderByIdService(Guid orderId)
    {
      return await _appDbContext.Orders
      .Include(_ => _.OrderProducts)
       .ThenInclude(_ => _.Product)
      .Include(_ => _.User)
      .FirstOrDefaultAsync(order => order.OrderId == orderId);
    }


    public async Task<Order> AddOrderService(NewOrderDto newOrder)
    {
      try
      {
        
        var userExists = await _appDbContext.Users.AnyAsync(u => u.UserId == newOrder.UserId);
        if (!userExists)
        {
          throw new InvalidOperationException("User does not exist.");
        }

        var productIds = newOrder.Products.Select(p => p.ProductId).ToList();
        var existingProducts = await _appDbContext.Products.Where(p => productIds.Contains(p.ProductId)).ToListAsync();

        if (existingProducts.Count != newOrder.Products.Count)
        {
          throw new InvalidOperationException("One or more products do not exist.");
        }

        Order order = new()
        {
          OrderId = Guid.NewGuid(),
          Date = DateTime.UtcNow,
          TotalPrice = newOrder.TotalPrice,
          UserId = newOrder.UserId,
          OrderStatus = "pending" 
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
      catch (DbUpdateException dbEx)
      {
       
        Console.WriteLine($"Database update exception: {dbEx.Message}, Inner Exception: {dbEx.InnerException?.Message}");
        throw new InvalidOperationException("Could not save the order to the database.", dbEx);
      }
      catch (Exception ex)
      {
       
        Console.WriteLine($"Exception: {ex.Message}, Inner Exception: {ex.InnerException?.Message}");
        throw new InvalidOperationException("There was an issue when processing the order.", ex);
      }
    }


   
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