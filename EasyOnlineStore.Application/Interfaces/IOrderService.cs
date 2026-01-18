using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderResponse>> GetAllAsync();
    Task<OrderResponse> GetByIdAsync(Guid id);
    Task<List<OrderResponse>> GetByPageAsync(int page, int pageSize);

    Task<OrderResponse> CreateOrderAsync(Guid cartId);
    Task<OrderResponse> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);
    Task<bool> DeleteOrderAsync(Guid orderId);
    Task<OrderResponse> CancelOrderAsync(Guid orderId);

}
