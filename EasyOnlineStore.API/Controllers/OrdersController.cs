using System.Security.Claims;
using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController(IOrderService orderService) : ControllerBase
{
    #region AllUsers

    // GET: api/orders/
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderResponse>> GetByUserId(Guid orderId)
    {
        var userId = GetUserIdFromToken();
        var order = await orderService.GetByUserIdAsync(userId, orderId);
        return Ok(order);
    }

    // GET: api/orders
    [HttpGet()]
    public async Task<ActionResult<List<OrderResponse>>> GetAllOrdersByUserId()
    {
        var userId = GetUserIdFromToken();
        var orders = await orderService.GetAllByUserIdAsync(userId);
        return Ok(orders);
    }
    
    // POST: api/orders/create/
    [HttpPost("create")]
    public async Task<ActionResult<OrderResponse>> CreateOrderByUserId()
    {
        var userId = GetUserIdFromToken();
        var createdOrder = await orderService.CreateOrderByUserIdAsync(userId);
        return CreatedAtAction(nameof(GetByUserId), new { orderId = createdOrder.Id }, createdOrder);
    }
    
    // PUT: api/orders/cancel/orderId
    [HttpPut("cancel/{orderId:guid}")]
    public async Task<ActionResult<OrderResponse>> CancelOrder(Guid orderId)
    {
        var userId = GetUserIdFromToken();
        var canceledOrder = await orderService.CancelOrderAsync(userId, orderId);
        return Ok(canceledOrder);

    }
    #endregion
    
    
    #region Admin & Developer
    
    // GET: api/orders/
    [HttpGet]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<List<OrderResponse>>> GetByPage([FromQuery] int page=1, [FromQuery] int pageSize=10)
    {
        var orders = await orderService.GetByPageAsync(page, pageSize);
        return Ok(orders);
    }
    
    // GET: api/orders/all
    [HttpGet("all")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<List<OrderResponse>>> GetAll()
    {
        var orders = await orderService.GetAllAsync();
        return Ok(orders);
    }
    
    // PATCH: /api/orders/admin/{orderId}?status=StatusName&userId=
    [HttpPatch("admin/{orderId:guid}")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(Guid orderId, 
        [FromQuery] OrderStatus status, 
        [FromQuery] Guid userId)
    {
        var updatedOrder = await orderService.UpdateOrderStatusByUserIdAsync(userId, orderId, status);
        return Ok(updatedOrder);
    }

    // DELETE: api/orders/admin/{orderId}?userId=
    [HttpDelete("admin/{orderId:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<bool>> DeleteOrder(Guid orderId, [FromQuery] Guid userId)
    {
        await orderService.DeleteOrderByUserIdAsync(userId, orderId);
        return NoContent();
    }

    // DELETE: api/orders/admin/all?userId=
    [HttpDelete("admin/all")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<bool>> DeleteAllOrders([FromQuery] Guid userId)
    {
        await orderService.DeleteAllByUserIdAsync(userId);
        return NoContent();
    }
    
    #endregion
    
    
    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new BadHttpRequestException("Invalid user ID in token");
        
        return userId;
    }
    
}
