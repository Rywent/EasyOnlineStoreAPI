using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private readonly EasyOnlineStoreDbContext _dbContext;

    public WarehouseRepository(EasyOnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await _dbContext.Warehouses
            .AsNoTracking()
            .Select(w => new Warehouse
            {
                Id = w.Id,
                Name = w.Name,
                Adress = w.Adress,
            })
            .ToListAsync();
    }
    public async Task<Warehouse?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Warehouses
            .Include(w => w.Products)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Warehouse> CreateAsync(Warehouse warehouse)
    {
        await _dbContext.Warehouses.AddAsync(warehouse);
        await _dbContext.SaveChangesAsync();
        return warehouse;
    }
    public async Task<Warehouse> UpdateAsync(Warehouse warehouse)
    {
        var existing = await _dbContext.Warehouses.FindAsync(warehouse.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Warehouse {warehouse.Id} not found.");

        _dbContext.Entry(existing).CurrentValues.SetValues(warehouse);
        await _dbContext.SaveChangesAsync();
        return warehouse;
    }
    public async Task<bool> RemoveAsync(Guid id)
    {
        var result = await _dbContext.Warehouses
            .Where(w => w.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    
}
