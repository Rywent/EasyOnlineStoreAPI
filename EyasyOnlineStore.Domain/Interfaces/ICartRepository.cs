using EasyOnlineStore.Domain.Models.Carts;

namespace EasyOnlineStore.Domain.Interfaces;

public interface ICartRepository
{
    Task<List<Cart>> GetAllAsync();
    Task<Cart?> GetByIdAsync(Guid id);
    Task<Cart?> AddItemToCartAsync(Guid cartId, CartItem item);
    Task<Cart?> RemoveItemFromCartAsync(Guid cartId, Guid itemId);
    Task<Cart?> ClearCartAsync(Guid cartId);
    Task<Cart> CreateAsync(Cart cart);
    Task<bool> RemoveAsync(Guid id);
    Task<Cart?> UpdateItemInCartAsync(Guid cartId, Guid itemId, int quantity);

}
