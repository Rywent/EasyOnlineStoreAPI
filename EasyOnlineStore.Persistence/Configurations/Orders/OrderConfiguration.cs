using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EasyOnlineStore.Domain.Models.Orders;
using EasyOnlineStore.Domain.Models.Users;

namespace EasyOnlineStore.Persistence.Configurations.Orders;


public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .HasMany(o => o.Items)
            .WithOne(p => p.Order)
            .HasForeignKey(k => k.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasOne<ApplicationUser>()
            .WithMany(u => u.Orders)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Restrict);


    }
}
