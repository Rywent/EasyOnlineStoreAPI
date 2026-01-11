using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    // GET: api/orders/
    [HttpGet]
    public async Task<ActionResult<List<OrderResponse>>> GetByPage([FromQuery] int page=1, [FromQuery] int pageSize=10)
    {
        var orders = await _orderService.GetByPageAsync(page, pageSize);
        return Ok(orders ?? []);
    }

    // GET: api/orders/all
    [HttpGet("all")]
    public async Task<ActionResult<List<OrderResponse>>> GetAll()
    {
        var orders = await _orderService.GetAllAsync();
        return Ok(orders ?? []);
    }

    // GET: api/orders/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<OrderResponse>> GetById(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        return Ok(order);
    }

    // POST: api/orders/cartId
    [HttpPost("{cartId:guid}")]
    public async Task<ActionResult<OrderResponse>> CreateOrder(Guid cartId)
    {
        var createdOrder = await _orderService.CreateOrderAsync(cartId);
        return CreatedAtAction(nameof(GetById), new { id = createdOrder.Id },createdOrder);
    }

    // PATCH: /api/orders/orderId?status=statusName
    [HttpPatch("{orderId:guid}")]
    public async Task<ActionResult<OrderResponse>> UpdateStatus(Guid orderId, [FromQuery] OrderStatus status)
    {
        var updatedOrder = await _orderService.UpdateOrderStatusAsync(orderId, status);
        return Ok(updatedOrder);
    }

    // DELETE: api/orders/id
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<bool>> DeleteOrder(Guid orderId)
    {
        var result = await _orderService.DeleteOrderAsync(orderId);
        return NoContent();
    }

    // PUT: api/orders/cancel/id
    [HttpPut("cancel/{id:guid}")]
    public async Task<ActionResult<OrderResponse>> CancelOrder(Guid id)
    {
        var order = await _orderService.GetByIdAsync(id);
        var canceledOrder = await _orderService.CancelOrderAsync(id);
        return Ok(canceledOrder);

    }

}
