using EasyOnlineStore.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class OrderService
{

    [LoggerMessage(
        EventId = 201,
        Level = LogLevel.Warning,
        Message = "Order with ID {OrderId} for user {UserId} not found")]
    static partial void LogOrderNotFound(ILogger logger, Guid orderId, Guid userId);


    [LoggerMessage(
        EventId = 202,
        Level = LogLevel.Warning,
        Message = "Order creation failed: Cart for user {UserId} was not found")]
    static partial void LogCartNotFound(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 203,
        Level = LogLevel.Warning,
        Message = "Order creation failed: User {UserId} tried to order from an empty cart")]
    static partial void LogCartEmpty(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 204,
        Level = LogLevel.Warning,
        Message = "Order creation failed: Product {ProductId} inside user {UserId} cart does not exist")]
    static partial void LogProductInCartNotFound(ILogger logger, Guid productId, Guid userId);


    [LoggerMessage(
        EventId = 205,
        Level = LogLevel.Warning,
        Message = "Order creation failed: Insufficient stock for product {ProductId} ({ProductName}). Requested: {RequestedQuantity}, Available: {AvailableStock}")]
    static partial void LogInsufficientStock(ILogger logger, Guid productId, string productName, int requestedQuantity, int availableStock);


    [LoggerMessage(
        EventId = 206,
        Level = LogLevel.Information,
        Message = "Order {OrderId} ({OrderNumber}) successfully created for user {UserId}")]
    static partial void LogOrderCreated(ILogger logger, Guid orderId, string orderNumber, Guid userId);


    [LoggerMessage(
        EventId = 207,
        Level = LogLevel.Error,
        Message = "Database failed to update order {OrderId}")]
    static partial void LogOrderUpdateFailed(ILogger logger, Guid orderId);


    [LoggerMessage(
        EventId = 208,
        Level = LogLevel.Information,
        Message = "Order {OrderId} status successfully updated to {Status}")]
    static partial void LogOrderStatusUpdated(ILogger logger, Guid orderId, OrderStatus status);


    [LoggerMessage(
        EventId = 209,
        Level = LogLevel.Warning,
        Message = "Failed to cancel order {OrderId}: order is in {CurrentStatus} status, but must be Pending")]
    static partial void LogOrderCancelFailedInvalidStatus(ILogger logger, Guid orderId, OrderStatus currentStatus);


    [LoggerMessage(
        EventId = 210,
        Level = LogLevel.Information,
        Message = "Order {OrderId} was successfully cancelled by user {UserId}. Stock was restored")]
    static partial void LogOrderCancelled(ILogger logger, Guid orderId, Guid userId);


    [LoggerMessage(
        EventId = 211,
        Level = LogLevel.Information,
        Message = "Order {OrderId} was successfully deleted by user {UserId}")]
    static partial void LogOrderDeleted(ILogger logger, Guid orderId, Guid userId);


    [LoggerMessage(
        EventId = 212,
        Level = LogLevel.Warning,
        Message = "Failed to delete all orders: User {UserId} does not have any orders")]
    static partial void LogDeleteAllOrdersFailedNoOrders(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 213,
        Level = LogLevel.Information,
        Message = "All orders for user {UserId} were successfully deleted")]
    static partial void LogAllOrdersDeleted(ILogger logger, Guid userId);
}