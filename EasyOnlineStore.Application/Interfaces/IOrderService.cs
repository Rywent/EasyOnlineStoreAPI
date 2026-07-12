using EasyOnlineStore.Application.DTOs.Responses.Order;
using EasyOnlineStore.Domain.Enums;

namespace EasyOnlineStore.Application.Interfaces;


public interface IOrderService

{

    Task<List<OrderResponse>> GetAllAsync();
    Task<OrderResponse> GetByUserIdAsync(Guid userId, Guid orderId);
    Task<List<OrderResponse>> GetAllByUserIdAsync(Guid userId);
    Task<List<OrderResponse>> GetByPageAsync(int page, int pageSize);
    Task<OrderResponse> CreateOrderByUserIdAsync(Guid userId);

    Task<OrderResponse> UpdateOrderStatusByUserIdAsync(Guid userId, Guid orderId, OrderStatus status);

    Task<bool> DeleteOrderByUserIdAsync(Guid userId, Guid orderId);
    Task<bool> DeleteAllByUserIdAsync(Guid userId);

    Task<OrderResponse> CancelOrderAsync(Guid userId, Guid orderId);



}