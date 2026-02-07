using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Common;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Entities.Communication;
using Workers.Domain.Entities.Companies;
using Workers.Domain.Entities.Locations;
using Workers.Domain.Entities.Payments;
using Workers.Domain.Entities.Reviews;
using Workers.Domain.Entities.Users;
using Workers.Domain.Entities.Workers;

namespace Workers.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<WorkerProfile> WorkerProfiles => Set<WorkerProfile>();
    public DbSet<WorkerPortfolioItem> WorkerPortfolioItems => Set<WorkerPortfolioItem>();
    public DbSet<WorkerMedia> WorkerMedia => Set<WorkerMedia>();
    public DbSet<WorkerCategory> WorkerCategories => Set<WorkerCategory>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<CompanyMember> CompanyMembers => Set<CompanyMember>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<WorkRequest> WorkRequests => Set<WorkRequest>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<ChatRoom> ChatRooms => Set<ChatRoom>();
    public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ReviewMedia> ReviewMedia => Set<ReviewMedia>();
    public DbSet<Payment> Payments => Set<Payment>();

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        HandleSoftDelete();
        return base.SaveChangesAsync(ct);
    }

    private void HandleSoftDelete()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is IBaseEntity);

        foreach (var entry in entries)
        {
            var entity = (IBaseEntity)entry.Entity;
            entry.State = EntityState.Modified;
            entity.IsDeleted = true;
            entity.DeletedAt = DateTime.UtcNow;
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        // Ensure Identity tables use snake_case if configured, though ApplyConfigurations or naming convention should handle it
    }
}
