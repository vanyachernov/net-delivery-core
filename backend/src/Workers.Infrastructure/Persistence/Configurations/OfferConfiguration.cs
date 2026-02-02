using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Communication;

namespace Workers.Infrastructure.Persistence.Configurations;

public class OfferConfiguration : IEntityTypeConfiguration<Offer>
{
    public void Configure(EntityTypeBuilder<Offer> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Property(o => o.ProposedPrice).HasColumnType("decimal(18,2)");
        builder.Property(o => o.Message).HasMaxLength(1000);

        builder.HasOne(o => o.WorkerProfile)
            .WithMany()
            .HasForeignKey(o => o.WorkerProfileId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(o => !o.IsDeleted);
    }
}
