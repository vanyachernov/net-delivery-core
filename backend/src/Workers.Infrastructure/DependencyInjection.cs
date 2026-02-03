using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Workers.Application.Common.Interfaces;
using Workers.Infrastructure.Persistence;
using Workers.Infrastructure.Persistence.Repositories;

namespace Workers.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, m => m.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
                   .UseSnakeCaseNamingConvention());
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        return services;
    }
}
