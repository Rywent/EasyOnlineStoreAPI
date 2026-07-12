using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Application.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> GetByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default);
    Task<List<OrderResponse>> GetOrdersByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default); 
    Task<List<OrderResponse>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    Task<List<OrderResponse>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status, int page, int pageSize, CancellationToken ct = default);
    Task<OrderResponse> CreateOrderByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<OrderResponse> UpdateOrderStatusByUserIdAsync(Guid userId, Guid orderId, OrderStatus status, CancellationToken ct = default);
    Task<OrderResponse> CancelOrderAsync(Guid userId, Guid orderId, CancellationToken ct = default);
    Task<bool> DeleteOrderByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default);
    Task<bool> DeleteAllByUserIdAsync(Guid userId, CancellationToken ct = default);
}