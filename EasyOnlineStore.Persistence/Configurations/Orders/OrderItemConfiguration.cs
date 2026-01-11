using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EasyOnlineStore.Domain.Models.Orders;


namespace EasyOnlineStore.Persistence.Configurations.Orders;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasOne(oi => oi.Product)
            .WithMany()
            .HasForeignKey(oi => oi.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Warehouse)
               .WithMany()
               .HasForeignKey(oi => oi.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
