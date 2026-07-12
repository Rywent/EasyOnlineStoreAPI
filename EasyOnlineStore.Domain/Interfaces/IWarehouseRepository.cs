using EasyOnlineStore.Domain.Models.Warehouses;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IWarehouseRepository
{
    public Task<List<Warehouse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    
    public Task<Warehouse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    public Task<Warehouse?> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default);
    public Task<List<Warehouse>> GetWarehousesByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    public Task<Warehouse> CreateAsync(Warehouse warehouse, CancellationToken ct = default);
    public Task<Warehouse> UpdateAsync(Warehouse warehouse, CancellationToken ct = default);
    public Task<bool> RemoveByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default);

    public Task<bool> CloseAllByUserIdAsync(Guid userId, CancellationToken ct = default);

}
