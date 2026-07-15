using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class ProductsService
{
    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Warning,
        Message = "Product with ID {ProductId} not found")]
    static partial void LogProductNotFound(ILogger logger, Guid productId);


    [LoggerMessage(
        EventId = 102,
        Level = LogLevel.Warning,
        Message = "Category with ID {CategoryId} not found during product creation")]
    static partial void LogCategoryNotFound(ILogger logger, Guid categoryId);


    [LoggerMessage(
        EventId = 103,
        Level = LogLevel.Warning,
        Message = "Failed to add product: Warehouse {WarehouseId} does not belong to seller {SellerId}")]
    static partial void LogWarehouseAccessDenied(ILogger logger, Guid warehouseId, Guid sellerId);


    [LoggerMessage(
        EventId = 104,
        Level = LogLevel.Information,
        Message = "Product with ID {ProductId} successfully created by seller {SellerId}")]
    static partial void LogProductCreated(ILogger logger, Guid productId, Guid sellerId);


    [LoggerMessage(
        EventId = 105,
        Level = LogLevel.Warning,
        Message = "Seller {SellerId} tried to update product {ProductId} without permission")]
    static partial void LogProductUpdateAccessDenied(ILogger logger, Guid productId, Guid sellerId);


    [LoggerMessage(
        EventId = 106,
        Level = LogLevel.Warning,
        Message = "Seller {SellerId} tried to move product {ProductId} to unauthorized warehouse {WarehouseId}")]
    static partial void LogProductMovedToWarehouseDenied(ILogger logger, Guid productId, Guid warehouseId, Guid sellerId);


    [LoggerMessage(
        EventId = 107,
        Level = LogLevel.Information,
        Message = "Product {ProductId} successfully updated by seller {SellerId}")]
    static partial void LogProductUpdated(ILogger logger, Guid productId, Guid sellerId);


    [LoggerMessage(
        EventId = 108,
        Level = LogLevel.Warning,
        Message = "Seller {SellerId} does not have permission to modify images for product {ProductId}")]
    static partial void LogProductImagesModifyAccessDenied(ILogger logger, Guid productId, Guid sellerId);


    [LoggerMessage(
        EventId = 109,
        Level = LogLevel.Information,
        Message = "Image {ImageId} successfully added to product {ProductId}")]
    static partial void LogProductImageAdded(ILogger logger, Guid imageId, Guid productId);


    [LoggerMessage(
        EventId = 110,
        Level = LogLevel.Information,
        Message = "Successfully added {Count} images to product {ProductId}")]
    static partial void LogProductMultipleImagesAdded(ILogger logger, int count, Guid productId);


    [LoggerMessage(
        EventId = 111,
        Level = LogLevel.Warning,
        Message = "Image {ImageId} was not found within product {ProductId}")]
    static partial void LogProductImageNotFound(ILogger logger, Guid imageId, Guid productId);


    [LoggerMessage(
        EventId = 112,
        Level = LogLevel.Information,
        Message = "Image {ImageId} successfully removed from product {ProductId}")]
    static partial void LogProductImageDeleted(ILogger logger, Guid imageId, Guid productId);


    [LoggerMessage(
        EventId = 113,
        Level = LogLevel.Warning,
        Message = "Seller {SellerId} tried to delete product {ProductId} without permission")]
    static partial void LogProductDeleteAccessDenied(ILogger logger, Guid productId, Guid sellerId);


    [LoggerMessage(
        EventId = 114,
        Level = LogLevel.Information,
        Message = "Product {ProductId} successfully deleted by seller {SellerId}")]
    static partial void LogProductDeleted(ILogger logger, Guid productId, Guid sellerId);
}