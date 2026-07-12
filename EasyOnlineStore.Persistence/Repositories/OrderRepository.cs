using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace EasyOnlineStore.Persistence.Repositories;

public class OrderRepository(EasyOnlineStoreDbContext dbContext) : IOrderRepository
{
    public async Task<List<Order>> GetAllAsync()
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Warehouse!)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByUserIdAsync(Guid userId, Guid orderId)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)!
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i!.Warehouse!)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.Id == orderId);
    }
        
    public async Task<List<Order>> GetOrdersByUserIdAsync(Guid userId)
    {
        return await dbContext.Orders
            .Where(o => o.UserId == userId)
            .AsNoTracking()
            .Include(o => o.Items)!
            .ThenInclude(i => i.Product)
            .ThenInclude(i => i!.Warehouse!)
            .ToListAsync();
    }

    public async Task<List<Order>> GetByPageAsync(int page, int pageSize)
    {
        return await dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Warehouse!)
            .OrderByDescending(o => o.CreatedDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
    public async Task<List<Order>> GetByFilterAsync(DateTime? createdDate, OrderStatus? status)
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

        query = query.OrderByDescending(o => o.CreatedDate);

        return await query.ToListAsync();
    }


    public async Task<Order> CreateAsync(Order order)
    {
        await dbContext.Orders.AddAsync(order);
        await dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<Order> UpdateAsync(Order order)
    {
        var existing = await dbContext.Orders.FindAsync(order.Id);
        if (existing == null) 
            throw new KeyNotFoundException($"Order {order.Id} not found");

        dbContext.Entry(existing).CurrentValues.SetValues(order);
        await dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<bool> RemoveByUserIdAsync(Guid userId, Guid orderId)
    {
        var result = await dbContext.Orders
            .Where(o => o.UserId == userId && o.Id == orderId)
            .ExecuteDeleteAsync();

        return result > 0;
    }

    public async Task<bool> RemoveAllByUserIdAsync(Guid userId)
    {
        var result = await dbContext.Orders
            .Where(o => o.UserId == userId)
            .ExecuteDeleteAsync();
        
        return result > 0;
    }

   
}
