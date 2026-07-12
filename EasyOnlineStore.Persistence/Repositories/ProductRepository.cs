using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class ProductRepository(EasyOnlineStoreDbContext dbContext) : IProductRepository
{
    public async Task<List<Product>> GetAllAsync()
    {
        return await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .OrderByDescending(p => p.Price)
            .ToListAsync();
    }
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await dbContext.Products
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

    }

    public async Task<List<Product>> GetByIdsAsync(Guid[] productIds)
    {
        return await dbContext.Products
            .Where(p => productIds.Contains(p.Id))
            .ToListAsync();

    }

    public async Task<List<Product>> GetProductsBySellerIdAsync(Guid sellerId)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .Where(p => p.SellerId == sellerId)
            .ToListAsync();
    }
    public async Task<List<Product>> GetByFilterAsync(string name, decimal price)
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

        return await query.ToListAsync();
    }
    public async Task<List<Product>> GetByPageAsync(int page, int pageSize)
    {
        return await dbContext.Products
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include (p => p.Images)
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<List<Product>> GetProductsByPageBySellerIdAsync(Guid sellerId, int page, int pageSize)
    {
        return await dbContext.Products
            .Where(s => s.SellerId == sellerId)
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include (p => p.Images)
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }


    public async Task<Product> CreateAsync(Product product)
    {
        await dbContext.Products.AddAsync(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        dbContext.Products.Update(product);
        await dbContext.SaveChangesAsync();
        return product;
    }

    public async Task UpdateRangeAsync(IEnumerable<Product> products)
    {
        dbContext.Products.UpdateRange(products);
        await dbContext.SaveChangesAsync();
    }
    
    public async Task<bool> RemoveAsync(Guid sellerId, Guid productId)
    {
        var result = await dbContext.Products
            .Where(p => p.SellerId == sellerId && p.Id == productId)
            .ExecuteDeleteAsync();

        return result > 0;
    }
}
