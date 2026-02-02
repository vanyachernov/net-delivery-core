using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Users;

namespace Workers.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        
        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);
            
        builder.HasIndex(u => u.Email).IsUnique();
        
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(20);

        builder.HasOne(u => u.WorkerProfile)
            .WithOne(wp => wp.User)
            .HasForeignKey<Domain.Entities.Workers.WorkerProfile>(wp => wp.UserId);

        builder.HasOne(u => u.Company)
            .WithOne(c => c.Owner)
            .HasForeignKey<Domain.Entities.Companies.Company>(c => c.OwnerId);
            
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}
