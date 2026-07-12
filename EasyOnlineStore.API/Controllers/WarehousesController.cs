using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[Authorize]
public class WarehousesController(IWarehouseService warehouseService) : BaseApiController
{
    #region AllUsers

    // GET api/warehouses/{warehouseId}
    [HttpGet("{warehouseId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<WarehouseResponse>> GetById(Guid warehouseId, CancellationToken ct = default)
    {
        var warehouse = await warehouseService.GetByIdAsync(warehouseId, ct);
        return Ok(warehouse);
    }

    // GET api/warehouses/user/{userId}?page=1&pageSize=10
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WarehouseResponse>>> GetWarehousesBySeller(
        Guid userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var warehouses = await warehouseService.GetWarehousesByUserIdAsync(userId, page, pageSize, ct);
        return Ok(warehouses);
    }

    #endregion
    
    #region Seller, Admin & Developer
    
    // GET api/warehouses/my?page=1&pageSize=10
    [HttpGet("my")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<WarehouseResponse>>> GetMyWarehouses(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var userId = GetUserIdFromToken();
        var warehouses = await warehouseService.GetWarehousesByUserIdAsync(userId, page, pageSize, ct);
        return Ok(warehouses);
    }
    
    // GET api/warehouses/my/{warehouseId}
    [HttpGet("my/{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> GetMyWarehouse(Guid warehouseId, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var warehouse = await warehouseService.GetWarehouseByUserIdAsync(userId, warehouseId, ct);
        return Ok(warehouse);
    }
    
    // POST api/warehouses
    [HttpPost]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> Create(WarehouseCreateRequest request, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var createdWarehouse = await warehouseService.CreateAsync(request, userId, ct);
        return CreatedAtAction(nameof(GetById), new { warehouseId = createdWarehouse.Id }, createdWarehouse);
    }
    
    // PATCH api/warehouses/{warehouseId}
    [HttpPatch("{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> Update(Guid warehouseId, WarehouseUpdateRequest request, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var updatedWarehouse = await warehouseService.UpdateAsync(warehouseId, request, userId, ct);
        return Ok(updatedWarehouse);
    }
    
    // POST api/warehouses/my/close
    [HttpPost("my/close")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult> CloseAllMyWarehouses(CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        await warehouseService.CloseWarehousesByUserIdAsync(userId, ct);
        return NoContent();
    }

    // DELETE api/warehouses/{warehouseId}
    [HttpDelete("{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult> Delete(Guid warehouseId, CancellationToken ct = default)
    {
        var userId = GetUserIdFromToken();
        var result = await warehouseService.DeleteByUserIdAsync(userId, warehouseId, ct);
        return result ? NoContent() : NotFound();
    }
    
    #endregion
    
    #region Admin & Developer
    
    // GET api/warehouses?page=1&pageSize=10
    [HttpGet]
    [Authorize(Roles = "Admin,Developer")]
    public async Task<ActionResult<List<WarehouseShortResponse>>> GetByPage(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10, 
        CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        var warehouses = await warehouseService.GetByPageAsync(page, pageSize, ct);
        return Ok(warehouses);
    }
    
    #endregion
}