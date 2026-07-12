using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;

namespace EasyOnlineStore.Application.Interfaces;

public interface ICartService
{
    Task<List<CartResponse>> GetAllAsync();
    Task<CartResponse> GetByUserIdAsync(Guid userId);
    Task<CartResponse> CreateCartAsync(Guid userId);
    Task<CartResponse> AddItemToCartByUserIdAsync(Guid userId, CartAddItemRequest request);
    Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId);
    Task<CartResponse> UpdateItemInCartByUserIdAsync(Guid userId, CartItemUpdateRequest request);
    Task<CartResponse> ClearCartByUserIdAsync(Guid userId);
    Task<bool> DeleteCartByUserIdAsync(Guid userId);
    
}
