using EasyOnlineStore.Domain.Models.Carts;

namespace EasyOnlineStore.Domain.Interfaces;

public interface ICartRepository
{
    Task<List<Cart>> GetAllAsync();
    Task<Cart?> GetByUserIdAsync(Guid userId);
    Task<Cart?> AddItemToCartByUserIdAsync(Guid userId, CartItem item);
    Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid itemId);
    Task<Cart?> ClearCartByUserIdAsync(Guid userId);
    Task<Cart> CreateAsync(Cart cart);
    Task<bool> RemoveByUserIdAsync(Guid userId);
    Task<Cart?> UpdateItemInCartByUserIdAsync(Guid userId, Guid itemId, int quantity);

}
