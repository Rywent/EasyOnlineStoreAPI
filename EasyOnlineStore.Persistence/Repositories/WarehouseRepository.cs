using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class WarehouseRepository(EasyOnlineStoreDbContext dbContext) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetAllAsync()
    {
        return await dbContext.Warehouses
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task<Warehouse?> GetByIdAsync(Guid id)
    {
        return await dbContext.Warehouses
            .Include(w => w.Products)
            .FirstOrDefaultAsync(w => w.Id == id);

    }
    public async Task<Warehouse?> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId)
    {
        return await dbContext.Warehouses
            .Include(w => w.Products)
            .FirstOrDefaultAsync(w => w.OwnerUserId == userId && w.Id == warehouseId);
    }
    
    public async Task<List<Warehouse>> GetWarehousesByUserIdAsync(Guid userId)
    {
        return await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId)
            .Include(w => w.Products)
            .ToListAsync();
    }

    public async Task<Warehouse> CreateAsync(Warehouse warehouse)
    {
        await dbContext.Warehouses.AddAsync(warehouse);
        await dbContext.SaveChangesAsync();
        return warehouse;
    }
    public async Task<Warehouse> UpdateAsync(Warehouse warehouse)
    {
        var existing = await dbContext.Warehouses.FindAsync(warehouse.Id);
        if (existing == null)
            throw new KeyNotFoundException($"Warehouse {warehouse.Id} not found.");

        dbContext.Entry(existing).CurrentValues.SetValues(warehouse);
        await dbContext.SaveChangesAsync();
        return warehouse;
    }
    public async Task<bool> CloseAllByUserIdAsync(Guid userId)
    {
        var affectedRows = await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId && w.IsActive)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.IsActive, false));

        return affectedRows > 0;
    }
    
    public async Task<bool> RemoveByUserIdAsync(Guid userId, Guid warehouseId)
    {
        var result = await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId && w.Id == warehouseId)
            .ExecuteDeleteAsync();

        return result > 0;
    }

}
