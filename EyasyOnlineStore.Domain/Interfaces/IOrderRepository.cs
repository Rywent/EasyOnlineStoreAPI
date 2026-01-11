using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Orders;
namespace EasyOnlineStore.Domain.Interfaces;

public interface IOrderRepository
{
    public Task<List<Order>> GetAllAsync();
    public Task<Order?> GetByIdAsync(Guid id);
    public Task<List<Order>> GetByPageAsync(int page, int pageSize);
    public Task<List<Order>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status);

    public Task<Order> CreateAsync(Order order);
    public Task<bool> RemoveAsync(Guid id);
    public Task<Order> UpdateAsync(Order order);
}
