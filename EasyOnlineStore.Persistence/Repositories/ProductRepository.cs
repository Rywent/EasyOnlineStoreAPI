using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class ProductRepository(EasyOnlineStoreDbContext dbContext) : IProductRepository
{
    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Products
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<List<Product>> GetByIdsAsync(Guid[] productIds, CancellationToken ct = default)
    {
        return await dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync(ct);
    }

    public async Task<List<Product>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<Product>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Products
            .Where(s => s.SellerId == sellerId)
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<Product>> GetByFilterAsync(string name, decimal price, int page, int pageSize, CancellationToken ct = default)
    {
        var query = dbContext.Products.AsNoTracking();

        if (!string.IsNullOrEmpty(name))
        {
            query = query.Where(p => p.Name.Contains(name));
        }
        if (price > 0)
        {
            query = query.Where(p => p.Price > price);
        }
        
        return await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Product> CreateAsync(Product product, CancellationToken ct = default)
    {
        await dbContext.Products.AddAsync(product, ct); 
        await dbContext.SaveChangesAsync(ct);
        return product;
    }

    public async Task<Product> UpdateAsync(Product product, CancellationToken ct = default)
    {
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync(ct);
        return product;
    }

    public async Task UpdateRangeAsync(IEnumerable<Product> products, CancellationToken ct = default)
    {
        dbContext.Products.UpdateRange(products);
        await dbContext.SaveChangesAsync(ct);
    }
    
    public async Task<bool> RemoveAsync(Guid sellerId, Guid productId, CancellationToken ct = default)
    {
        var result = await dbContext.Products
            .Where(p => p.SellerId == sellerId && p.Id == productId)
            .ExecuteDeleteAsync(ct);

        return result > 0;
    }
}