using EasyOnlineStore.Domain.Models.Carts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyOnlineStore.Persistence.Configurations.Carts;

public class CartConfiguration : IEntityTypeConfiguration<Cart>
{
    public void Configure(EntityTypeBuilder<Cart> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        builder.HasMany(c => c.Items)
               .WithOne(ci => ci.Cart)
               .HasForeignKey(ci => ci.CartId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
