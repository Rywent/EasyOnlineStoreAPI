using AutoMapper;
using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Application.Interfaces;
using EasyOnlineStore.Application.Exceptions;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Orders;

namespace EasyOnlineStore.Application.Services;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    ICartRepository cartRepository,
    IMapper mapper)
    : IOrderService
{
    public async Task<OrderResponse> GetByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByUserIdAsync(userId, orderId, ct);
        if (order == null) 
            throw new NotFoundException(nameof(Order), orderId);

        return mapper.Map<OrderResponse>(order);
    }

    public async Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        var orders = await orderRepository.GetOrdersByUserIdAsync(userId, page, pageSize, ct);
        return mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<List<OrderResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        var orders = await orderRepository.GetByPageAsync(page, pageSize, ct);
        return mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<List<OrderResponse>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status, int page, int pageSize, CancellationToken ct = default)
    {
        var orders = await orderRepository.GetByFilterAsync(createdDate, status, page, pageSize, ct);
        return mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<OrderResponse> CreateOrderByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var cart = await cartRepository.GetByUserIdAsync(userId, ct);
        
        if (cart == null)
            throw new NotFoundException($"Cart for user with ID '{userId}' was not found.");

        if (cart.Items == null || !cart.Items.Any())
            throw new InvalidOperationException("Cannot create an order from an empty cart.");

        var productIds = cart.Items.Select(i => i.ProductId).Distinct().ToArray();
        var products = await productRepository.GetByIdsAsync(productIds, ct);
        var productsDict = products.ToDictionary(p => p.Id);

        foreach (var cartItem in cart.Items)
        {
            if (!productsDict.TryGetValue(cartItem.ProductId, out var product))
                throw new NotFoundException(nameof(Product), cartItem.ProductId);
            if (product.Stock < cartItem.Quantity)
                throw new InsufficientStockException(product, cartItem.Quantity);
        }

        var newOrder = new Order
        {
             Id = Guid.NewGuid(),
             UserId = userId,
             OrderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd-HHmmss}",
             CreatedDate = DateTime.UtcNow,
             Status = OrderStatus.Pending,
             Items = cart.Items
            .Where(cartItem => productsDict.ContainsKey(cartItem.ProductId))
            .Select(cartItem => new OrderItem
            {
                Id = Guid.NewGuid(),
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                UnitPrice = productsDict[cartItem.ProductId].Price,
                WarehouseId = productsDict[cartItem.ProductId].WarehouseId
            }).ToList()
        };

        foreach (var orderItem in newOrder.Items)
        {
            productsDict[orderItem.ProductId].Stock -= orderItem.Quantity;
        }

        await productRepository.UpdateRangeAsync(productsDict.Values.ToList(), ct);

        await orderRepository.CreateAsync(newOrder, ct);
        await cartRepository.ClearCartByUserIdAsync(userId, ct);

        return mapper.Map<OrderResponse>(newOrder);
    }

    public async Task<OrderResponse> UpdateOrderStatusByUserIdAsync(Guid userId, Guid orderId, OrderStatus status, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByUserIdAsync(userId, orderId, ct);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        order.Status = status;

        var updatedOrder = await orderRepository.UpdateAsync(order, ct);

        if (updatedOrder == null)
            throw new InvalidOperationException("Failed to update order");
            
        return mapper.Map<OrderResponse>(updatedOrder);
    }

    public async Task<OrderResponse> CancelOrderAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByUserIdAsync(userId, orderId, ct);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);

        if (order.Status != OrderStatus.Pending)
            throw new InvalidOrderStatusException(order.Status, "cancel");

        var productIds = order.Items.Select(i => i.ProductId).Distinct().ToArray();
        var products = await productRepository.GetByIdsAsync(productIds, ct);

        foreach (var item in order.Items)
        {
            var product = products.FirstOrDefault(p => p.Id == item.ProductId);
            if (product != null)
            {
                product.Stock += item.Quantity;
            }
        }

        await productRepository.UpdateRangeAsync(products, ct);
        
        order.Status = OrderStatus.Cancelled;
        var cancelledOrder = await orderRepository.UpdateAsync(order, ct);

        return mapper.Map<OrderResponse>(cancelledOrder);
    }

    public async Task<bool> DeleteOrderByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        var order = await orderRepository.GetByUserIdAsync(userId, orderId, ct);
        if (order == null)
            throw new NotFoundException(nameof(Order), orderId);
        
        return await orderRepository.RemoveByUserIdAsync(userId, orderId, ct);
    }
    
    public async Task<bool> DeleteAllByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var deleted = await orderRepository.RemoveAllByUserIdAsync(userId, ct);
        
        if (!deleted)
            throw new NotFoundException($"User with {userId} id hasn't placed a single order yet");

        return deleted;
    }
}