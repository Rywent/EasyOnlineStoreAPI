using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[Authorize]
public class OrdersController(IOrderService orderService) : BaseApiController
{
    #region AllUsers

    // GET: api/orders/{orderId}
    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderResponse>> GetByUserId(Guid orderId, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var order = await orderService.GetByUserIdAsync(userId, orderId, ct);
        return Ok(order);
    }

    // GET: api/orders?page=1&pageSize=10
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetOrdersByUserId(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var userId = GetUserIdFromToken();
        var orders = await orderService.GetOrdersByUserIdAsync(userId, page, pageSize, ct);
        return Ok(orders);
    }
    
    // POST: api/orders/create
    [HttpPost("create")]
    public async Task<ActionResult<OrderResponse>> CreateOrderByUserId(CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var createdOrder = await orderService.CreateOrderByUserIdAsync(userId, ct);
        return CreatedAtAction(nameof(GetByUserId), new { orderId = createdOrder.Id }, createdOrder);
    }
    
    // PUT: api/orders/cancel/{orderId}
    [HttpPut("cancel/{orderId:guid}")]
    public async Task<ActionResult<OrderResponse>> CancelOrder(Guid orderId, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var canceledOrder = await orderService.CancelOrderAsync(userId, orderId, ct);
        return Ok(canceledOrder);
    }
    #endregion
    
    #region Admin & Developer
    
    // GET: api/orders/admin?page=1&pageSize=10
    [HttpGet("admin")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<List<OrderResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var orders = await orderService.GetByPageAsync(page, pageSize, ct);
        return Ok(orders);
    }
    
    
    // PATCH: /api/orders/admin/{orderId}?status=OrderStatus&userId=.
    [HttpPatch("admin/{orderId:guid}")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(
        Guid orderId, 
        [FromQuery] OrderStatus status, 
        [FromQuery] Guid userId, 
        CancellationToken ct = default)
    {
        var updatedOrder = await orderService.UpdateOrderStatusByUserIdAsync(userId, orderId, status, ct);
        return Ok(updatedOrder);
    }

    // DELETE: api/orders/admin/{orderId}?userId=
    [HttpDelete("admin/{orderId:guid}")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<bool>> DeleteOrder(Guid orderId, [FromQuery] Guid userId, CancellationToken ct = default)
    {
        await orderService.DeleteOrderByUserIdAsync(userId, orderId, ct);
        return NoContent();
    }

    // DELETE: api/orders/admin/all?userId=
    [HttpDelete("admin/all")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<bool>> DeleteAllOrders([FromQuery] Guid userId, CancellationToken ct = default)
    {
        await orderService.DeleteAllByUserIdAsync(userId, ct);
        return NoContent();
    }
    
    #endregion
}