using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Orders;
namespace EasyOnlineStore.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default);
    public Task<Order?> GetByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default);
    public Task<List<Order>> GetOrdersByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default);
    Task<List<Order>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status, int page, int pageSize, CancellationToken ct = default);

    public Task<Order> CreateAsync(Order order, CancellationToken ct = default);
    public Task<Order> UpdateAsync(Order order, CancellationToken ct = default);
    public Task<bool> RemoveByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default);
    public Task<bool> RemoveAllByUserIdAsync(Guid userId, CancellationToken ct = default);
    
}
