using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Interfaces;
using EasyOnlineStore.Domain.Models.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;

namespace EasyOnlineStore.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly EasyOnlineStoreDbContext _dbContext;

    public OrderRepository(EasyOnlineStoreDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Order>> GetAllAsync()
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)
                .ThenInclude(o => o.Product)
                    .ThenInclude(o => o.Warehouse!)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Orders
            .AsNoTracking()
            .Include(o => o.Items)!
                .ThenInclude(i => i.Product)
                .ThenInclude(i => i.Warehouse!)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<List<Order>> GetByPageAsync(int page, int pageSize)
    {
        return await _dbContext.Orders
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
        var query = _dbContext.Orders.AsNoTracking();

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
        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<Order> UpdateAsync(Order order)
    {
        var existing = await _dbContext.Orders.FindAsync(order.Id);
        if (existing == null) 
            throw new KeyNotFoundException($"Order {order.Id} not found");

        _dbContext.Entry(existing).CurrentValues.SetValues(order);
        await _dbContext.SaveChangesAsync();
        return order;
    }
    public async Task<bool> RemoveAsync(Guid id)
    {
        var result = await _dbContext.Orders
            .Where(o => o.Id == id)
            .ExecuteDeleteAsync();

        return result > 0;
    }

   
}
