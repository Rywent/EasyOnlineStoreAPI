using EasyOnlineStore.Domain.Models.Carts;

namespace EasyOnlineStore.Domain.Interfaces;

public interface ICartRepository
{
    Task<List<Cart>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Cart?> AddItemToCartByUserIdAsync(Guid userId, CartItem item, CancellationToken ct = default);
    Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId, CancellationToken ct = default);
    Task<Cart?> ClearCartByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default);
    Task<bool> RemoveByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Cart?> UpdateItemInCartByUserIdAsync(Guid userId, Guid itemId, int quantity, CancellationToken ct = default);

}
