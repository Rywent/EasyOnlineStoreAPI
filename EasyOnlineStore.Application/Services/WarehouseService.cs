using AutoMapper;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Warehouses;

namespace EasyOnlineStore.Application.Services;

public class WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper) : IWarehouseService
{
    public async Task<List<WarehouseShortResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var warehouses = await warehouseRepository.GetByPageAsync(page, pageSize, ct);
        return mapper.Map<List<WarehouseShortResponse>>(warehouses);
    }

    public async Task<WarehouseResponse> GetByIdAsync(Guid warehouseId, CancellationToken ct = default)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(warehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return mapper.Map<WarehouseResponse>(warehouse);
    }
    
    public async Task<WarehouseResponse> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default)
    {
        var warehouse = await warehouseRepository.GetWarehouseByUserIdAsync(userId, warehouseId, ct);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return mapper.Map<WarehouseResponse>(warehouse);
    }

    public async Task<List<WarehouseResponse>> GetWarehousesByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        // Передаем параметры пагинации пользователя прямо в репозиторий
        var warehouses = await warehouseRepository.GetWarehousesByUserIdAsync(userId, page, pageSize, ct);
        if (warehouses == null || !warehouses.Any())
            throw new NotFoundException($"User with ID {userId} has no warehouses on this page");

        return mapper.Map<List<WarehouseResponse>>(warehouses);
    }

    public async Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request, Guid userId, CancellationToken ct = default)
    {
        var warehouse = mapper.Map<Warehouse>(request);
        warehouse.OwnerUserId = userId;
        
        var createdWarehouse = await warehouseRepository.CreateAsync(warehouse, ct);
        return mapper.Map<WarehouseResponse>(createdWarehouse);
    }

    public async Task<WarehouseResponse> UpdateAsync(Guid warehouseId, WarehouseUpdateRequest request, Guid userId, CancellationToken ct = default)
    {
        var existingWarehouse = await warehouseRepository.GetWarehouseByUserIdAsync(userId, warehouseId, ct);
        if (existingWarehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        mapper.Map(request, existingWarehouse);
        existingWarehouse.OwnerUserId = userId;
        
        var updatedWarehouse = await warehouseRepository.UpdateAsync(existingWarehouse, ct);
        return mapper.Map<WarehouseResponse>(updatedWarehouse);
    }

    public async Task<bool> CloseWarehousesByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var closed = await warehouseRepository.CloseAllByUserIdAsync(userId, ct);
        if (!closed)
            throw new NotFoundException($"User with ID {userId} has no active warehouses");

        return true;
    }
    
    public async Task<bool> DeleteByUserIdAsync(Guid userId, Guid warehouseId, CancellationToken ct = default)
    {
        var deleted = await warehouseRepository.RemoveByUserIdAsync(userId, warehouseId, ct);
        
        if (!deleted)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return deleted;
    }
}