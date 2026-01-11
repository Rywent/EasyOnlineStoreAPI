using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    public CartsController(ICartService cartService)
    {
        _cartService = cartService;
    }

    // GET: api/carts
    [HttpGet]
    public async Task<ActionResult<List<CartResponse>>> GetAll()
    {
        var carts = await _cartService.GetAllAsync();
        return Ok(carts);
    }

    // GET: api/carts/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CartResponse>> GetById(Guid id)
    {
        var cart = await _cartService.GetByIdAsync(id);
        return Ok(cart);
    }

    // POST: api/carts
    [HttpPost]
    public async Task<ActionResult<CartResponse>> CreateCart()
    {
        var createdCart = await _cartService.CreateCartAsync();
        return CreatedAtAction(nameof(GetById), new { id = createdCart.CartId }, createdCart);
    }

    // POST: api/carts/cartId/items
    [HttpPost("{cartId:guid}/items")]
    public async Task<ActionResult<CartResponse>> AddCartItem(Guid cartId, CartAddItemRequest request)
    {
        var cart = await _cartService.AddItemToCartAsync(cartId, request);
        return Ok(cart);
    }

    // PATCH api/carts/cartId/items/itemId
    [HttpPatch("{cartId:guid}/items/")]
    public async Task<ActionResult<CartResponse>> UpdateCartItem(Guid cartId, CartItemUpdateRequest request)
    {
        var cart = await _cartService.UpdateItemInCartAsync(cartId, request);
        return Ok(cart);
    }

    // DELETE api/carts/cartId/items/itemId
    [HttpDelete("{cartId:guid}/items/{itemId:guid}")]
    public async Task<ActionResult<CartResponse>> RemoveCartItem(Guid cartId, Guid itemId)
    {
        var cart = await _cartService.RemoveItemFromCartAsync(cartId, itemId);
        return Ok(cart);
    }

    //POST api/carts/cartId/items/clear
    [HttpPost("{cartId:guid}/items/clear")]
    public async Task<ActionResult<CartResponse>> ClearCart(Guid cartId)
    {
        var cart = await _cartService.ClearCartAsync(cartId);
        return Ok(cart);
    }

    // DELETE api/carts/id
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCart(Guid id)
    {
        var result = await _cartService.DeleteCartAsync(id);
        return result ? NoContent() : NotFound();

    }


}
