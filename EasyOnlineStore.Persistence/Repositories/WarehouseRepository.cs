using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Warehouses;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class WarehouseRepository(EasyOnlineStoreDbContext dbContext) : IWarehouseRepository
{
    public async Task<List<Warehouse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Warehouses
            .AsNoTracking()
            .OrderBy(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }
    
    public async Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Warehouses
            .Include(w => w.Products)
            .FirstOrDefaultAsync(w => w.Id == id, ct);
    }

    public async Task<Warehouse?> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default)
    {
        return await dbContext.Warehouses
            .Include(w => w.Products)
            .FirstOrDefaultAsync(w => w.OwnerUserId == userId && w.Id == warehouseId, ct);
    }
    
    public async Task<List<Warehouse>> GetWarehousesByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId)
            .AsNoTracking()
            .Include(w => w.Products)
            .OrderBy(w => w.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Warehouse> CreateAsync(Warehouse warehouse, CancellationToken ct = default)
    {
        await dbContext.Warehouses.AddAsync(warehouse, ct);
        await dbContext.SaveChangesAsync(ct);
        return warehouse;
    }

    public async Task<Warehouse> UpdateAsync(Warehouse warehouse, CancellationToken ct = default)
    {
        dbContext.Warehouses.Update(warehouse);
        await dbContext.SaveChangesAsync(ct);
        return warehouse;
    }

    public async Task<bool> CloseAllByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var affectedRows = await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId && w.IsActive)
            .ExecuteUpdateAsync(s => s.SetProperty(w => w.IsActive, false), ct);

        return affectedRows > 0;
    }
    
    public async Task<bool> RemoveByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default)
    {
        var result = await dbContext.Warehouses
            .Where(w => w.OwnerUserId == userId && w.Id == warehouseId)
            .ExecuteDeleteAsync(ct);
        return result > 0;
    }
}