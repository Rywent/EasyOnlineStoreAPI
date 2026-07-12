using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;

namespace EasyOnlineStore.Application.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseShortResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<WarehouseResponse> GetByIdAsync(Guid warehouseId, CancellationToken ct = default);
    Task<WarehouseResponse> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default);
    Task<List<WarehouseResponse>> GetWarehousesByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    
    Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request, Guid userId, CancellationToken ct = default);
    Task<WarehouseResponse> UpdateAsync(Guid warehouseId, WarehouseUpdateRequest request, Guid userId, CancellationToken ct = default);

    Task<bool> DeleteByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default);
    Task<bool> CloseWarehousesByUserIdAsync(Guid userId, CancellationToken ct = default);
}