using System.Security.Claims;
using EasyOnlineStore.Application.DTOs.Requests.Warehouse;
using EasyOnlineStore.Application.DTOs.Responses.Warehouse;
using EasyOnlineStore.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyOnlineStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WarehousesController(IWarehouseService warehouseService) : ControllerBase
{
    #region AllUsers

    // GET api/warehouses/warehouseId
    [HttpGet("{warehouseId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<WarehouseResponse>> GetById(Guid warehouseId)
    {
        var warehouse = await warehouseService.GetByIdAsync(warehouseId);
        return Ok(warehouse);
    }

    // GET api/warehouses/user/userId
    [HttpGet("user/{userId:guid}")]
    [AllowAnonymous]
    public async Task<ActionResult<List<WarehouseResponse>>> GetWarehousesBySeller(Guid userId)
    {
        var warehouses = await warehouseService.GetWarehousesByUserIdAsync(userId);
        return Ok(warehouses);
    }

    #endregion
    
    #region SellerAdminDeveloper
    
    // GET api/warehouses/my
    [HttpGet("my")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<List<WarehouseResponse>>> GetMyWarehouses()
    {
        var userId = GetUserIdFromToken();
        var warehouses = await warehouseService.GetWarehousesByUserIdAsync(userId);
        return Ok(warehouses);
    }
    
    // GET api/warehouses/my/warehouseId
    [HttpGet("my/{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> GetMyWarehouse(Guid warehouseId)
    {
        var userId = GetUserIdFromToken();
        var warehouse = await warehouseService.GetWarehouseByUserIdAsync(userId, warehouseId);
        return Ok(warehouse);
    }
    
    // POST api/warehouses
    [HttpPost]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> Create(WarehouseCreateRequest request)
    {
        var userId = GetUserIdFromToken();
        var createdWarehouse = await warehouseService.CreateAsync(request, userId);
        return Ok(createdWarehouse);
    }
    
    // PATH api/warehouses/warehouseId
    [HttpPatch("{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult<WarehouseResponse>> Update(Guid warehouseId, WarehouseUpdateRequest request)
    {
        var userId = GetUserIdFromToken();
        var updatedWarehouse = await warehouseService.UpdateAsync(warehouseId, request, userId);
        return Ok(updatedWarehouse);
    }
    
    [HttpPost("my/close")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult> CloseAllMyWarehouses()
    {
        var userId = GetUserIdFromToken();
        await warehouseService.CloseWarehousesByUserIdAsync(userId);
        return NoContent();
    }
    

    // DELETE api/warehouses/warehouseId
    [HttpDelete("{warehouseId:guid}")]
    [Authorize(Roles = "Seller,Admin,Developer")]
    public async Task<ActionResult> Delete(Guid warehouseId)
    {
        var userId = GetUserIdFromToken();
        var result = await warehouseService.DeleteByUserIdAsync(userId, warehouseId);
        return result ? NoContent() : NotFound();
    }
    
    #endregion
    
    
    
    #region AdminDeveloper
    
    // GET api/warehouses
    [Authorize(Roles = "Admin,Developer")]
    [HttpGet]
    public async Task<ActionResult<List<WarehouseResponse>>> GetAll([FromQuery] int page=1, [FromQuery] int pageSize=10)
    {
        if (pageSize <= 0) pageSize = 10;
        if (page < 1) page = 1;

        var warehouses = await warehouseService.GetAllAsync(page, pageSize);
        return Ok(warehouses);
    }
    
    #endregion
    

    

    

    
    
    private Guid GetUserIdFromToken()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
                          ?? User.FindFirst("sub")?.Value;
        
        if (string.IsNullOrEmpty(userIdClaim))
            throw new UnauthorizedAccessException("User ID not found in token");
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            throw new BadHttpRequestException("Invalid user ID in token");
        
        return userId;
    }
}
