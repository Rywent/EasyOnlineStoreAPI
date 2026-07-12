using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[Authorize]
public class CartsController(ICartService cartService) : BaseApiController
{
    #region AllUsers 

    // GET: api/carts
    [HttpGet]
    public async Task<ActionResult<CartResponse>> GetByUserId(CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.GetByUserIdAsync(userId, ct);
        return Ok(cart);
    }

    // POST: api/carts/items/add
    [HttpPost("items/add")]
    public async Task<ActionResult<CartResponse>> AddCartItem(CartAddItemRequest request, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.AddItemToCartByUserIdAsync(userId, request, ct);
        return Ok(cart);
    }

    // PATCH api/carts/items/update
    [HttpPatch("items/update")]
    public async Task<ActionResult<CartResponse>> UpdateCartItem(CartItemUpdateRequest request, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.UpdateItemInCartByUserIdAsync(userId, request, ct);
        return Ok(cart);
    }

    // DELETE api/carts/items/delete/{productId}
    [HttpDelete("items/delete/{productId:guid}")]
    public async Task<ActionResult<CartResponse>> RemoveCartItem(Guid productId, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.RemoveItemFromCartByUserIdAsync(userId, productId, ct);
        return Ok(cart);
    }

    // POST api/carts/items/clear
    [HttpPost("items/clear")]
    public async Task<ActionResult<CartResponse>> ClearCart(CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.ClearCartByUserIdAsync(userId, ct);
        return Ok(cart);
    }
    
    #endregion
    
    #region Admin and Developer
    
    // GET: api/carts/all?page=1&pageSize=10
    [HttpGet("all")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<CartResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; 

        var carts = await cartService.GetByPageAsync(page, pageSize, ct);
        return Ok(carts);
    }
    
    // POST: api/carts/create/userId
    [HttpPost("create/{userId:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<CartResponse>> CreateCart(Guid userId, CancellationToken ct = default)
    {
        var createdCart = await cartService.CreateCartAsync(userId, ct);
        return CreatedAtAction(nameof(GetByUserId), null, createdCart);
    }
    
    // DELETE api/carts/{userId}
    [HttpDelete("{userId:guid}")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<IActionResult> DeleteCart(Guid userId, CancellationToken ct = default)
    {
        var result = await cartService.DeleteCartByUserIdAsync(userId, ct);
        return result ? NoContent() : NotFound();
    }
    
    #endregion
}