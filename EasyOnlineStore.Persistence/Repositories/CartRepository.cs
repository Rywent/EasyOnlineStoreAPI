using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class CartRepository : ICartRepository
{
    private readonly EasyOnlineStoreDbContext _dbContext;

    public CartRepository(EasyOnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Cart>> GetAllAsync()
    {
        return await _dbContext.Carts
            .AsNoTracking()
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public async Task<Cart?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        await _dbContext.Carts.AddAsync(cart);
        await _dbContext.SaveChangesAsync();
        return cart;

    }
    public async Task<Cart?> AddItemToCartAsync(Guid cartId, CartItem item)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
            return null;

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == item.ProductId);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            item.CartId = cartId;
            cart.Items.Add(item);
        }

        await _dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<bool> RemoveItemFromCartAsync(Guid cartId, Guid productId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
            return false;

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem == null)
            return false;

        cart.Items.Remove(existingItem);
        await _dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<Cart?> ClearCartAsync(Guid cartId)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null)
            return null;

        cart.Items.Clear();
        await _dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<Cart?> UpdateItemInCartAsync(Guid cartId, Guid productId, int quantity)
    {
        var cart = await _dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.Id == cartId);

        if (cart == null) return null;

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem == null) return null;

        existingItem.Quantity = quantity;

        await _dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<bool> RemoveAsync(Guid id)
    {
        var result = await _dbContext.Carts
            .Where(c => c.Id == id)
            .ExecuteDeleteAsync();
        return result > 0;
    }

    
}
