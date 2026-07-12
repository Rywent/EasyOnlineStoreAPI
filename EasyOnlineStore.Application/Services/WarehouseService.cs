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
    public async Task<List<WarehouseShortResponse>> GetAllAsync(int page, int pageSize)
    {
        var warehouses = await warehouseRepository.GetAllAsync();
        var paged = warehouses
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return mapper.Map<List<WarehouseShortResponse>>(paged);
    }

    public async Task<WarehouseResponse> GetByIdAsync(Guid warehouseId)
    {
        var warehouse = await warehouseRepository.GetByIdAsync(warehouseId);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return mapper.Map<WarehouseResponse>(warehouse);
    }
    
    public async Task<WarehouseResponse> GetWarehouseByUserIdAsync(Guid userId, Guid warehouseId)
    {
        var warehouse = await warehouseRepository.GetWarehouseByUserIdAsync(userId, warehouseId);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return mapper.Map<WarehouseResponse>(warehouse);
    }

    public async Task<List<WarehouseResponse>> GetWarehousesByUserIdAsync(Guid userId)
    {
        var warehouse = await warehouseRepository.GetWarehousesByUserIdAsync(userId);
        if (warehouse == null)
            throw new NotFoundException($"User with ID {userId} has no warehouses");

        return mapper.Map<List<WarehouseResponse>>(warehouse);
    }

    public async Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request, Guid userId)
    {
        var warehouse = mapper.Map<Warehouse>(request);
        warehouse.OwnerUserId = userId;
        var createdWarehouse = await warehouseRepository.CreateAsync(warehouse);
        return mapper.Map<WarehouseResponse>(createdWarehouse);

    }

    public async Task<WarehouseResponse> UpdateAsync(Guid warehouseId, WarehouseUpdateRequest request, Guid userId)
    {
        var existingWarehouse = await warehouseRepository.GetWarehouseByUserIdAsync(userId, warehouseId);
        if(existingWarehouse == null)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        mapper.Map(request, existingWarehouse);
        existingWarehouse.OwnerUserId = userId;
        
        var updatedWarehouse = await warehouseRepository.UpdateAsync(existingWarehouse);
        return mapper.Map<WarehouseResponse>(updatedWarehouse);
    }

    public async Task<bool> CloseWarehousesByUserIdAsync(Guid userId)
    {
        var closed = await warehouseRepository.CloseAllByUserIdAsync(userId);
        if (!closed)
            throw new NotFoundException($"User with ID {userId} has no active warehouses");

        return true;
    }
    
    public async Task<bool> DeleteByUserIdAsync(Guid userId, Guid warehouseId)
    {
        var deleted =  await warehouseRepository.RemoveByUserIdAsync(userId, warehouseId);
        
        if(!deleted)
            throw new NotFoundException(nameof(Warehouse), warehouseId);

        return deleted;
    }
}
