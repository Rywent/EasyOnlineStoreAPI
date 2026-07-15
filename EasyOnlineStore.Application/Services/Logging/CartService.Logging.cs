using Microsoft.Extensions.Logging;

namespace EasyOnlineStore.Application.Services;

public partial class CartService
{
    [LoggerMessage(
        EventId = 301,
        Level = LogLevel.Warning,
        Message = "Cart for user with ID {UserId} was not found")]
    static partial void LogCartNotFound(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 302,
        Level = LogLevel.Warning,
        Message = "Failed to add item to cart: Product {ProductId} not found. Requested by user {UserId}")]
    static partial void LogProductNotFoundForCart(ILogger logger, Guid productId, Guid userId);


    [LoggerMessage(
        EventId = 303,
        Level = LogLevel.Warning,
        Message = "Failed to add item to cart: Insufficient stock for product {ProductId}. Requested: {RequestedQuantity}, Available: {AvailableStock}. User: {UserId}")]
    static partial void LogInsufficientProductStockForCart(ILogger logger, Guid productId, int requestedQuantity, int availableStock, Guid userId);


    [LoggerMessage(
        EventId = 304,
        Level = LogLevel.Information,
        Message = "Product {ProductId} (Quantity: {Quantity}) was successfully added to user {UserId} cart")]
    static partial void LogItemAddedToCart(ILogger logger, Guid productId, int quantity, Guid userId);


    [LoggerMessage(
        EventId = 305,
        Level = LogLevel.Information,
        Message = "Item {ItemId} was successfully removed from user {UserId} cart")]
    static partial void LogItemRemovedFromCart(ILogger logger, Guid itemId, Guid userId);


    [LoggerMessage(
        EventId = 306,
        Level = LogLevel.Information,
        Message = "Quantity for product {ProductId} was successfully updated to {Quantity} in user {UserId} cart")]
    static partial void LogItemQuantityUpdatedInCart(ILogger logger, Guid productId, int quantity, Guid userId);


    [LoggerMessage(
        EventId = 307,
        Level = LogLevel.Information,
        Message = "Cart for user {UserId} was successfully cleared")]
    static partial void LogCartCleared(ILogger logger, Guid userId);


    [LoggerMessage(
        EventId = 308,
        Level = LogLevel.Information,
        Message = "New cart {CartId} was successfully created for user {UserId}")]
    static partial void LogCartCreated(ILogger logger, Guid cartId, Guid userId);


    [LoggerMessage(
        EventId = 309,
        Level = LogLevel.Information,
        Message = "Cart for user {UserId} was successfully deleted from the database")]
    static partial void LogCartDeleted(ILogger logger, Guid userId);
}