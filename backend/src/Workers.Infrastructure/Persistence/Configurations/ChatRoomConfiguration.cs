using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Workers.Domain.Entities.Communication;

namespace Workers.Infrastructure.Persistence.Configurations;

public class ChatRoomConfiguration : IEntityTypeConfiguration<ChatRoom>
{
    public void Configure(EntityTypeBuilder<ChatRoom> builder)
    {
        builder.HasKey(cr => cr.Id);

        builder.HasOne(cr => cr.WorkRequest)
            .WithMany()
            .HasForeignKey(cr => cr.WorkRequestId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(cr => !cr.IsDeleted);
    }
}
