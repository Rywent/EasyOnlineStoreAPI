using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Orders;
namespace EasyOnlineStore.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllAsync();
    public Task<Order?> GetByUserIdAsync(Guid userId, Guid orderId);
    public Task<List<Order>> GetOrdersByUserIdAsync(Guid userId);
    public Task<List<Order>> GetByPageAsync(int page, int pageSize);
    public Task<List<Order>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status);

    public Task<Order> CreateAsync(Order order);
    public Task<bool> RemoveByUserIdAsync(Guid userId, Guid orderId);
    public Task<bool> RemoveAllByUserIdAsync(Guid userId);

    public Task<Order> UpdateAsync(Order order);
}
