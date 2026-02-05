using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Companies;

namespace Workers.Infrastructure.Persistence.Configurations;

public class CompanyMemberConfiguration : IEntityTypeConfiguration<CompanyMember>
{
    public void Configure(EntityTypeBuilder<CompanyMember> builder)
    {
        builder.HasKey(cm => cm.Id);

        builder.HasOne(cm => cm.Company)
            .WithMany(c => c.Members)
            .HasForeignKey(cm => cm.CompanyId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(cm => cm.User)
            .WithMany()
            .HasForeignKey(cm => cm.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(cm => !cm.IsDeleted);
    }
}
