using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EasyOnlineStore.Domain.Models;


namespace EasyOnlineStore.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .HasOne(p => p.Warehouse)
            .WithMany(p => p.Products)
            .HasForeignKey(p => p.WarehouseId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
        
    }
}

