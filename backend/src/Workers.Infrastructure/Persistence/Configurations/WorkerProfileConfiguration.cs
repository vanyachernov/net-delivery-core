using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Workers;

namespace Workers.Infrastructure.Persistence.Configurations;

public class WorkerProfileConfiguration : IEntityTypeConfiguration<WorkerProfile>
{
    public void Configure(EntityTypeBuilder<WorkerProfile> builder)
    {
        builder.HasKey(wp => wp.Id);

        builder.Property(wp => wp.Rating)
            .HasPrecision(3, 2);

        builder.HasMany(wp => wp.PortfolioItems)
            .WithOne(pi => pi.WorkerProfile)
            .HasForeignKey(pi => pi.WorkerProfileId);

        builder.HasMany(wp => wp.Media)
            .WithOne(m => m.WorkerProfile)
            .HasForeignKey(m => m.WorkerProfileId);

        builder.HasMany(wp => wp.Categories)
            .WithOne(wc => wc.WorkerProfile)
            .HasForeignKey(wc => wc.WorkerProfileId);

        builder.HasOne(wp => wp.Location)
            .WithMany()
            .HasForeignKey(wp => wp.LocationId);
            
        builder.HasQueryFilter(wp => !wp.IsDeleted);
    }
}
