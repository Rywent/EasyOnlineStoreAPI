using System.Security.Claims;
using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartsController(ICartService cartService) : ControllerBase
{

    #region AllUsers 

    // GET: api/carts
    [HttpGet()]
    public async Task<ActionResult<CartResponse>> GetByUserId()
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.GetByUserIdAsync(userId);
        return Ok(cart);
    }
    

    // POST: api/carts/cartId/items
    [HttpPost("items/add")]
    public async Task<ActionResult<CartResponse>> AddCartItem(CartAddItemRequest request)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.AddItemToCartByUserIdAsync(userId, request);
        return Ok(cart);
    }

    // PATCH api/carts/cartId/items/itemId
    [HttpPatch("items/update")]
    public async Task<ActionResult<CartResponse>> UpdateCartItem(CartItemUpdateRequest request)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.UpdateItemInCartByUserIdAsync(userId, request);
        return Ok(cart);
    }

    // DELETE api/carts/cartId/items/itemId
    [HttpDelete("items/delete/{productId:guid}")]
    public async Task<ActionResult<CartResponse>> RemoveCartItem(Guid productId)
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.RemoveItemFromCartByUserIdAsync(userId, productId);
        return Ok(cart);
    }

    //POST api/carts/cartId/items/clear
    [HttpPost("items/clear")]
    public async Task<ActionResult<CartResponse>> ClearCart()
    {
        var userId = GetUserIdFromToken();
        var cart = await cartService.ClearCartByUserIdAsync(userId);
        return Ok(cart);
    }
    
    #endregion
    
    #region Admin and Developer
    
    // GET: api/carts/all
    [HttpGet("all")]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<CartResponse>>> GetAll()
    {
        var carts = await cartService.GetAllAsync();
        return Ok(carts);
    }
    
    // POST: api/carts/create/userId
    [HttpPost("create/{userId:guid}")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<ActionResult<CartResponse>> CreateCart(Guid userId)
    {
        var createdCart = await cartService.CreateCartAsync(userId);
        return CreatedAtAction(nameof(GetByUserId), null, createdCart);
    }
    
    // DELETE api/carts/id
    [HttpDelete("{userId:guid}")]
    [Authorize(Roles = "Admin, Developer")]
    public async Task<IActionResult> DeleteCart(Guid userId)
    {
        var result = await cartService.DeleteCartByUserIdAsync(userId);
        return result ? NoContent() : NotFound();

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
