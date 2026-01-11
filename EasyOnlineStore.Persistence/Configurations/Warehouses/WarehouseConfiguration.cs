using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using EasyOnlineStore.Domain.Models.Warehouses;


namespace EasyOnlineStore.Persistence.Configurations.Warehouses;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder
            .Property(w => w.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
