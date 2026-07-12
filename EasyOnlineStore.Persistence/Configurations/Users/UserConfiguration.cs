using System.Text.Json;
using EasyOnlineStore.Domain.Enums;
using EasyOnlineStore.Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EasyOnlineStore.Persistence.Configurations.Users;

public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(p => p.Id)
            .HasDefaultValueSql("gen_random_uuid()");

        var converter = new ValueConverter<ICollection<UserRole>, string>(
            v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
            v => JsonSerializer.Deserialize<ICollection<UserRole>>(v, (JsonSerializerOptions?)null) ?? new List<UserRole>()
        );
        var comparer = new ValueComparer<ICollection<UserRole>>(
            (c1, c2) => c1!.SequenceEqual(c2!),
            c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
            c => c.ToList()
        );

        builder.Property(u => u.Roles)
            .HasConversion(converter)
            .HasColumnType("jsonb")
            .Metadata.SetValueComparer(comparer);

    }
}