using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class WarehouseService
{
    #region Logger Messages
    
    [LoggerMessage(
        EventId = 501, 
        Level = LogLevel.Warning, 
        Message = "Warehouse with ID: {WarehouseId} not found")]  
    static partial void LogWarehouseNotFound(ILogger logger, Guid warehouseId);


    [LoggerMessage(
        EventId = 502, 
        Level = LogLevel.Warning, 
        Message = "Warehouse with ID: {WarehouseId} not found for User: {UserId}")]  
    static partial void LogWarehouseNotFoundForUser(ILogger logger, Guid warehouseId, Guid userId);


    [LoggerMessage(
        EventId = 503, 
        Level = LogLevel.Warning, 
        Message = "User with ID {UserId} has no warehouses on this page")]  
    static partial void LogNoWarehousesForUser(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 504, 
        Level = LogLevel.Information, 
        Message = "Created Warehouse with ID: {WarehouseId} successfully for User: {UserId}")]  
    static partial void LogWarehouseCreated(ILogger logger, Guid warehouseId, Guid userId);


    [LoggerMessage(
        EventId = 505, 
        Level = LogLevel.Information, 
        Message = "Updated Warehouse with ID: {WarehouseId} successfully for User: {UserId}")]  
    static partial void LogWarehouseUpdated(ILogger logger, Guid warehouseId, Guid userId);


    [LoggerMessage(
        EventId = 506, 
        Level = LogLevel.Warning, 
        Message = "User with ID: {UserId} has no active warehouses to close")]  
    static partial void LogNoActiveWarehousesToClose(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 507, 
        Level = LogLevel.Information, 
        Message = "All user (ID: {UserId}) warehouses have been closed successfully")]  
    static partial void LogAllWarehousesClosed(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 508, 
        Level = LogLevel.Warning, 
        Message = "Failed to delete warehouse. Warehouse with ID: {WarehouseId} not found or does not belong to User: {UserId}")]  
    static partial void LogWarehouseDeleteFailed(ILogger logger, Guid warehouseId, Guid userId);


    [LoggerMessage(
        EventId = 509, 
        Level = LogLevel.Information, 
        Message = "Warehouse with ID: {WarehouseId} was deleted successfully by User: {UserId}")]  
    static partial void LogWarehouseDeleted(ILogger logger, Guid warehouseId, Guid userId);

    #endregion
}