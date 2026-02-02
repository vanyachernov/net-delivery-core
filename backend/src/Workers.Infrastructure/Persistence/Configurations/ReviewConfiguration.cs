using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Reviews;

namespace Workers.Infrastructure.Persistence.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Comment).HasMaxLength(2000);

        builder.HasOne(r => r.WorkRequest)
            .WithMany()
            .HasForeignKey(r => r.WorkRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Reviewer)
            .WithMany()
            .HasForeignKey(r => r.ReviewerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.Reviewee)
            .WithMany()
            .HasForeignKey(r => r.RevieweeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(r => r.Media)
            .WithOne(m => m.Review)
            .HasForeignKey(m => m.ReviewId);

        builder.HasQueryFilter(r => !r.IsDeleted);
    }
}
