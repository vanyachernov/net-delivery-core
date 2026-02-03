using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Reviews;

namespace Workers.Infrastructure.Persistence.Configurations;

public class ReviewMediaConfiguration : IEntityTypeConfiguration<ReviewMedia>
{
    public void Configure(EntityTypeBuilder<ReviewMedia> builder)
    {
        builder.HasKey(rm => rm.Id);

        builder.HasQueryFilter(rm => !rm.IsDeleted);
    }
}
