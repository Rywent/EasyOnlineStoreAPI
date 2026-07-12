using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence.Repositories;

public class OrderRepository(EasyOnlineStoreDbContext dbContext) : IOrderRepository
{
    public async Task<Order?> GetByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i!.Warehouse)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId, ct);
    }
        
    public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Orders
            .Where(o => o.UserId == userId)
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i!.Warehouse)
            .OrderByDescending(o => o.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<Order>> GetByPageAsync(int page, int pageSize, CancellationToken ct = default)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                    .ThenInclude(o => o!.Warehouse)
            .OrderByDescending(o => o.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<List<Order>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status, int page, int pageSize, CancellationToken ct = default)
    {
        var query = dbContext.Orders.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        if (createdDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate.Date == createdDate.Value.Date);
        }
        
        return await query
            .OrderByDescending(o => o.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<Order> CreateAsync(Order order, CancellationToken ct = default)
    {
        await dbContext.Orders.AddAsync(order, ct);
        await dbContext.SaveChangesAsync(ct);
        return order;
    }

    public async Task<Order> UpdateAsync(Order order, CancellationToken ct = default)
    {
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(ct);
        return order;
    }

    public async Task<bool> RemoveByUserIdAsync(Guid userId, Guid orderId, CancellationToken ct = default)
    {
        var result = await dbContext.Orders
            .Where(o => o.UserId == userId && o.Id == orderId)
            .ExecuteDeleteAsync(ct);

        return result > 0;
    }

    public async Task<bool> RemoveAllByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        var result = await dbContext.Orders
            .Where(o => o.UserId == userId)
            .ExecuteDeleteAsync(ct);
        
        return result > 0;
    }
}