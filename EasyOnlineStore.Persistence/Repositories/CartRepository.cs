using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Carts;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class CartRepository(EasyOnlineStoreDbContext dbContext) : ICartRepository
{
    public async Task<List<Cart>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Carts
            .AsNoTracking()
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .OrderBy(c => c.Id) 
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Cart?> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .SingleOrDefaultAsync(c => c.UserId == userId, ct);
    }

    public async Task<Cart> CreateAsync(Cart cart, CancellationToken ct = default)
    {
        cart.User = null;
        
        await dbContext.Carts.AddAsync(cart, ct);
        await dbContext.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<Cart?> AddItemToCartByUserIdAsync(Guid userId, CartItem item, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

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

        await dbContext.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<bool> RemoveItemFromCartByUserIdAsync(Guid userId, Guid productId, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cart is null)
            return false;

        var existingItem = cart.Items
            .FirstOrDefault(i => i.ProductId == productId);

        if (existingItem == null)
            return false;

        cart.Items.Remove(existingItem);
        await dbContext.SaveChangesAsync(ct);

        return true;
    }

    public async Task<Cart?> ClearCartByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cart == null)
            return null;

        cart.Items.Clear();
        await dbContext.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<Cart?> UpdateItemInCartByUserIdAsync(Guid userId, Guid productId, int quantity, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(c => c.Items)
                .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId, ct);

        if (cart == null) return null;

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem == null) return null;

        existingItem.Quantity = quantity;

        await dbContext.SaveChangesAsync(ct);
        return cart;
    }

    public async Task<bool> RemoveByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var result = await dbContext.Carts
            .Where(c => c.UserId == userId)
            .ExecuteDeleteAsync(ct);
        return result > 0;
    }
}