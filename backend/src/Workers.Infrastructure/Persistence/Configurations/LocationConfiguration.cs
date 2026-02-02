using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Locations;

namespace Workers.Infrastructure.Persistence.Configurations;

public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name).IsRequired().HasMaxLength(100);
        builder.Property(l => l.Region).IsRequired().HasMaxLength(100);
        builder.Property(l => l.District).IsRequired().HasMaxLength(100);
        builder.Property(l => l.PostalCode).IsRequired().HasMaxLength(10);

        builder.HasQueryFilter(l => !l.IsDeleted);
    }
}
