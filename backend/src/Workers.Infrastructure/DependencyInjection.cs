using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using Workers.Application.Common.Interfaces;
using Workers.Infrastructure.Caching;
using Workers.Infrastructure.Persistence;
using Workers.Infrastructure.Repositories;

namespace Workers.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.AddNpgsqlDbContext<ApplicationDbContext>("DefaultConnection",
            configureDbContextOptions: options =>
            {
                options.UseSnakeCaseNamingConvention();
            });
            
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());
        
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.AddRedisClient("redis");
        builder.Services.AddSingleton<ICategoryCache, CategoryCache>();
    }
}





