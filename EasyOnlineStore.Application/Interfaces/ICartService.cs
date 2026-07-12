using EasyOnlineStore.Application.DTOs.Requests.Cart;
using EasyOnlineStore.Application.DTOs.Responses.Cart;

namespace EasyOnlineStore.Application.Interfaces;

public interface ICartService
{
    Task<List<CartResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<CartResponse> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<CartResponse> AddItemToCartByUserIdAsync(Guid userId, CartAddItemRequest request, CancellationToken ct = default);
    Task<CartResponse> UpdateItemInCartByUserIdAsync(Guid userId, CartItemUpdateRequest request, CancellationToken ct = default);
    Task<CartResponse> ClearCartByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<CartResponse> CreateCartAsync(Guid userId, CancellationToken ct = default);
    
    Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId, CancellationToken ct = default);
    Task<bool> DeleteCartByUserIdAsync(Guid userId, CancellationToken ct = default);
}