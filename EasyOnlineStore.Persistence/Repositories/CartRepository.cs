using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class CartRepository(EasyOnlineStoreDbContext dbContext) : ICartRepository
{
    public async Task<List<Cart>> GetAllAsync()
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .ToListAsync();
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId)
    {
        return await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(c=> c.UserId == userId);
    }

    public async Task<Cart> CreateAsync(Cart cart)
    {
        await dbContext.Carts.AddAsync(cart);
        await dbContext.SaveChangesAsync();
        return cart;

    }
    public async Task<Cart?> AddItemToCartByUserIdAsync(Guid userId, CartItem item)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

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
            item.CartId = cart.Id;
            cart.Items.Add(item);
        }

        await dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid productId)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart is null)
            return false;

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem == null)
            return false;

        cart.Items.Remove(existingItem);
        await dbContext.SaveChangesAsync();

        return true;
    }
    public async Task<Cart?> ClearCartByUserIdAsync(Guid userId)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return null;

        cart.Items.Clear();
        await dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<Cart?> UpdateItemInCartByUserIdAsync(Guid userId, Guid productId, int quantity)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null) return null;

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem == null) return null;

        existingItem.Quantity = quantity;

        await dbContext.SaveChangesAsync();
        return cart;
    }
    public async Task<bool> RemoveByUserIdAsync(Guid userId)
    {
        var result = await dbContext.Carts
            .Where(c => c.UserId == userId)
            .ExecuteDeleteAsync();
        return result > 0;
    }

    
}
