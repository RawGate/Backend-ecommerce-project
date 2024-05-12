using System.Security.Claims;
using backend_teamwork.Services;
using backend_teamwork1.Controllers;
using backend_teamwork1.DTOs;
using backend_teamwork1.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend_teamwork.Controllers
{
  [ApiController]
  [Route("api/orders")]
  public class OrderController : ControllerBase
  {
    private readonly OrderService _orderService;
    private readonly AuthService _authService;

    public OrderController(OrderService orderService, AuthService authService)
    {
      _orderService = orderService;
      _authService = authService;
    }

    [Authorize(Roles = "admin")]
    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
      try
      {
        var orders = await _orderService.GetAllOrdersService();
        if (orders.ToList().Count < 1)
        {
          return ApiResponse.NotFound("There is no Orders");
        }
        return ApiResponse.Success(orders, "All orders are returned successfully");
      }
      catch (Exception e)
      {
        return ApiResponse.ServerError(e.Message);
      }
    }

    // api/orders/userOrders  [HttpGet("userOrders")]
    // Get specific user orders 
    [Authorize]
    [HttpGet("userOrders")]
    public async Task<IActionResult> GetUserOrders()
    {
      try
      {
        var userIdString = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        Console.WriteLine($"{userIdString}");
        if (string.IsNullOrEmpty(userIdString))
        {
          return ApiResponse.UnAuthorized("User Id is missing from token");
        }

        if (!Guid.TryParse(userIdString, out Guid userId))
        {
          return ApiResponse.BadRequest("Invalid User Id");
        }

        var userOrders = await _orderService.GetUserOrdersService(userId);

        if (userOrders.ToList().Count < 1)
        {
          return ApiResponse.NotFound("There is no Orders");
        }
        return ApiResponse.Success(userOrders, "All orders are returned successfully");
      }
      catch (Exception e)
      {
        return ApiResponse.ServerError(e.Message);
      }
    }

    [HttpGet("{orderId:guid}")]
    public async Task<IActionResult> GetOrderById(Guid orderId)
    {
      try
      {
        var order = await _orderService.GetOrderByIdService(orderId);
        if (order == null)
          throw new InvalidOperationException();

        return ApiResponse.Success(order);
      }
      catch (InvalidOperationException e)
      {
        Console.WriteLine($"Exception : {e.Message}");
        return ApiResponse.NotFound("Order is not found");
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception : {e.Message}");
        return ApiResponse.ServerError("There was an issue when fetching the order data");
      }
    }

    [HttpPost]
    public async Task<IActionResult> AddOrder(NewOrderDto newOrder)
    {
      try
      {
        if (!ModelState.IsValid)
        {
          return ApiResponse.BadRequest("Invalid Order Data");
        }

        if (newOrder == null)
        {
          return ApiResponse.BadRequest("Invalid Order Data"); //exception
        }

        var order = await _orderService.AddOrderService(newOrder);
        return ApiResponse.Created(order, "Order is created successfully");
      }
      catch (Exception e)
      {
        Console.WriteLine($"Exception : {e.Message}");
        return ApiResponse.ServerError("There was an issue when fetching the order");
      }
    }

    [HttpPut("{orderId:guid}")]
    public async Task<IActionResult> UpdateOrder(Guid orderId, UpdateOrderDto updateOrder)
    {
      try
      {
        var order = await _orderService.GetOrderByIdService(orderId);
        if (order == null)
          return ApiResponse.NotFound("Order is not found");

        await _orderService.UpdateOrderService(orderId, updateOrder);
        return ApiResponse.Success(order, "Order is updated successfully");
      }
      catch (Exception e)
      {
        return ApiResponse.ServerError(e.Message);
      }
    }

    [HttpDelete("{orderId:guid}")]
    public async Task<IActionResult> DeleteOrder(Guid orderId)
    {
      try
      {
        var order = await _orderService.GetOrderByIdService(orderId);
        if (order == null)
          return ApiResponse.NotFound("Order is not found");

        await _orderService.DeleteOrderService(orderId);
        return ApiResponse.Success("Order is deleted successfully");
      }
      catch (Exception e)
      {
        return ApiResponse.ServerError(e.Message);
      }
    }


  }
}