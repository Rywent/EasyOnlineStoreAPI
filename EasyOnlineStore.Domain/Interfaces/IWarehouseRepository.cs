using EasyOnlineStore.Domain.Models.Warehouses;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IWarehouseRepository
{
    public Task<List<Warehouse>> GetAllAsync();
    
    public Task<Warehouse?> GetByIdAsync(Guid id);
    public Task<Warehouse?> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId);
    public Task<List<Warehouse>> GetWarehousesByUserIdAsync(Guid userId);
    public Task<Warehouse> CreateAsync(Warehouse warehouse);
    public Task<Warehouse> UpdateAsync(Warehouse warehouse);
    public Task<bool> RemoveByUserIdAsync(Guid userId, Guid warehouseId);

    public Task<bool> CloseAllByUserIdAsync(Guid userId);

}
