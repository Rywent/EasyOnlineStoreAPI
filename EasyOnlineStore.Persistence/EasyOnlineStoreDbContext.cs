using EasyOnlineStore.Domain.Models.Carts;
using EasyOnlineStore.Domain.Models.Orders;
using EasyOnlineStore.Domain.Models.Products;
using EasyOnlineStore.Domain.Models.Warehouses;
using EasyOnlineStore.Persistence.Configurations.Products;
using EasyOnlineStore.Persistence.Configurations.Carts;
using EasyOnlineStore.Persistence.Configurations.Orders;
using EasyOnlineStore.Persistence.Configurations.Warehouses;

using Microsoft.EntityFrameworkCore;

namespace EasyOnlineStore.Persistence;

public class EasyOnlineStoreDbContext : DbContext
{
    public EasyOnlineStoreDbContext(DbContextOptions<EasyOnlineStoreDbContext> options)
        : base(options) { }

    public DbSet<Warehouse> Warehouses { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
        modelBuilder.ApplyConfiguration(new CartConfiguration());
        modelBuilder.ApplyConfiguration(new CartItemConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderItemConfiguration());


        base.OnModelCreating(modelBuilder);
    }


}
