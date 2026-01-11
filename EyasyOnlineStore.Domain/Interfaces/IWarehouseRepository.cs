using EasyOnlineStore.Domain.Models;

namespace EasyOnlineStore.Domain.Interfaces;

public interface IWarehouseRepository
{
    public Task<List<Warehouse>> GetAllAsync();
    public Task<Warehouse?> GetByIdAsync(Guid id);
    public Task<Warehouse> CreateAsync(Warehouse warehouse);
    public Task<Warehouse> UpdateAsync(Warehouse warehouse);
    public Task<bool> RemoveAsync(Guid id);

}
