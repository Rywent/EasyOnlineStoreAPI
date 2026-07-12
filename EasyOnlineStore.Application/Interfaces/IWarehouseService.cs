using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;

namespace EasyOnlineStore.Application.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseShortResponse>> GetAllAsync(int page, int pageSize);
    Task<WarehouseResponse> GetByIdAsync(Guid warehouseId);
    Task<WarehouseResponse> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId);
    Task<List<WarehouseResponse>> GetWarehousesByUserIdAsync(Guid userId);
    
    Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request, Guid userId);
    Task<WarehouseResponse> UpdateAsync(Guid warehouseId, WarehouseUpdateRequest request, Guid userId);

    Task<bool> DeleteByUserIdAsync(Guid userId, Guid warehouseId);
    Task<bool> CloseWarehousesByUserIdAsync(Guid userId);

}
