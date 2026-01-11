using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;
    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    // GET api/warehouses
    [HttpGet]
    public async Task<ActionResult<List<WarehouseResponse>>> GetAll([FromQuery] int page=1, [FromQuery] int pageSize=10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;

        var warehouses = await _warehouseService.GetAllAsync(page, pageSize);
        return Ok(warehouses);
    }

    // GET api/warehouses/id
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<WarehouseResponse>> GetById(Guid id)
    {
        var warehouse = await _warehouseService.GetByIdAsync(id);
        return Ok(warehouse);
    }

    // POST api/warehouses
    [HttpPost]
    public async Task<ActionResult<WarehouseResponse>> Create(WarehouseCreateRequest request)
    {
        var createdWarehouse = await _warehouseService.CreateAsync(request);
        return Ok(createdWarehouse);
    }

    // PATH api/warehouses/id
    [HttpPatch("{id:guid}")]
    public async Task<ActionResult<WarehouseResponse>> Update(Guid id, WarehouseUpdateRequest request)
    {
        var updatedWarehouse = await _warehouseService.UpdateAsync(id, request);
        return Ok(updatedWarehouse);
    }

    // DELETE api/warehouses/id
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var result = await _warehouseService.DeleteAsync(id);
        return result ? NoContent() : NotFound();
    }
}
