using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EasyOnlineStoreDbContext _dbContext;
    public ProductRepository(EasyOnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .OrderByDescending(p => p.Price)
            .ToListAsync();
    }
    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Products
            .Include(p => p.Warehouse)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);

    }
    public async Task<List<Product>> GetByIdsAsync(Guid[] ids)
    {
        return await _dbContext.Products
            .Where(p => ids.Contains(p.Id))
            .ToListAsync();
    }
    public async Task<List<Product>> GetByFilterAsync(string name, decimal price)
    {
        var query = _dbContext.Products.AsNoTracking();

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
        return await _dbContext.Products
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
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> RemoveAsync(Guid id)
    {
        var result = await _dbContext.Products
            .Where(p => p.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }
}
