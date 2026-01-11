using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;

namespace EasyOnlineStore.Application.Interfaces;

public interface IWarehouseService
{
    Task<List<WarehouseResponse>> GetAllAsync(int page, int pageSize);
    Task<WarehouseResponse> GetByIdAsync(Guid id);
    
    Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request);
    Task<WarehouseResponse> UpdateAsync(Guid id, WarehouseUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);

}
