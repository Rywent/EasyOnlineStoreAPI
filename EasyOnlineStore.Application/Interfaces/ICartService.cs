using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;

namespace EasyOnlineStore.Application.Interfaces;

public interface ICartService
{
    Task<List<CartResponse>> GetAllAsync();
    Task<CartResponse> GetByIdAsync(Guid id);
    Task<CartResponse> CreateCartAsync();
    Task<CartResponse> AddItemToCartAsync(Guid cartId, CartAddItemRequest request);
    Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid itemId);
    Task<CartResponse> UpdateItemInCartAsync(Guid cartId, CartItemUpdateRequest request);
    Task<CartResponse> ClearCartAsync(Guid cartId);
    Task<bool> DeleteCartAsync(Guid cartId);
    
}
