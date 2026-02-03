using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Domain.Entities.Categories;
using Workers.Domain.Entities.Communication;
using Workers.Domain.Entities.Companies;
using Workers.Domain.Entities.Locations;
using Workers.Domain.Entities.Payments;
using Workers.Domain.Entities.Reviews;
using Workers.Domain.Entities.Users;
using Workers.Domain.Entities.Workers;

namespace Workers.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext,IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
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

    public override Task<int> SaveChangesAsync(CancellationToken ct = default) => base.SaveChangesAsync(ct);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
