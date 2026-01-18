using AutoMapper;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Warehouses;


namespace EasyOnlineStore.Application.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;
    private readonly IMapper _mapper;
    public WarehouseService(IWarehouseRepository warehouseRepository, IMapper mapper)
    {
        _warehouseRepository = warehouseRepository;
        _mapper = mapper;
    }
    public async Task<List<WarehouseShortResponse>> GetAllAsync(int page, int pageSize)
    {
        var warehouses = await _warehouseRepository.GetAllAsync();
        var paged = warehouses
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return _mapper.Map<List<WarehouseShortResponse>>(paged);
    }

    public async Task<WarehouseResponse> GetByIdAsync(Guid id)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), id);

        return _mapper.Map<WarehouseResponse>(warehouse);
    }

    public async Task<WarehouseResponse> CreateAsync(WarehouseCreateRequest request)
    {
        var warehouse = _mapper.Map<Warehouse>(request);
        var createdWarehouse = await _warehouseRepository.CreateAsync(warehouse);
        return _mapper.Map<WarehouseResponse>(createdWarehouse);

    }

    public async Task<WarehouseResponse> UpdateAsync(Guid id, WarehouseUpdateRequest request)
    {
        var existingWarehouse = await _warehouseRepository.GetByIdAsync(id);
        if(existingWarehouse == null)
            throw new NotFoundException(nameof(Warehouse), id);

        _mapper.Map(request, existingWarehouse);

        var updatedWarehouse = await _warehouseRepository.UpdateAsync(existingWarehouse);
        return _mapper.Map<WarehouseResponse>(updatedWarehouse);
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        var warehouse = await _warehouseRepository.GetByIdAsync(id);
        if (warehouse == null)
            throw new NotFoundException(nameof(Warehouse), id);
        return await _warehouseRepository.RemoveAsync(id);
    }
}
