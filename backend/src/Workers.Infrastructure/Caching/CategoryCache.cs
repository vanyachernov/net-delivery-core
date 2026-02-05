using System.Text.Json;
using StackExchange.Redis;
using Workers.Application.Categories.DTOs;
using Workers.Application.Categories.Queries;
using Workers.Application.Common.Interfaces;


namespace Workers.Infrastructure.Caching;

public class CategoryCache(IConnectionMultiplexer redis) : ICategoryCache
{
    private readonly IDatabase _db = redis.GetDatabase();
    private static readonly TimeSpan Ttl = TimeSpan.FromMinutes(10);
    private const string VersionKey = "categories:version";

    private async Task<long> GetVersionAsync()
    {
        var v = await _db.StringGetAsync(VersionKey);
        if (v.IsNullOrEmpty) { await _db.StringSetAsync(VersionKey, 1); return 1; }
        return (long)v!;
    }

    private async Task<string> BuildKey(Guid? parentId, CategoryLoadMode mode)
    {
        var v = await GetVersionAsync();
        var parent = parentId?.ToString() ?? "null";
        return $"categories:v{v}:mode:{mode}:parent:{parent}";
    }

    public async Task<List<CategoryDto>?> GetAsync(Guid? parentId, CategoryLoadMode mode, CancellationToken ct)
    {
        var key = await BuildKey(parentId, mode);
        var json = await _db.StringGetAsync(key);
        return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<List<CategoryDto>>(json.ToString());
    }

    public async Task SetAsync(Guid? parentId, CategoryLoadMode mode, List<CategoryDto> data, CancellationToken ct)
    {
        var key = await BuildKey(parentId, mode);
        var json = System.Text.Json.JsonSerializer.Serialize(data);
        await _db.StringSetAsync(key, json, Ttl);
    }

    public async Task InvalidateAsync(CancellationToken ct)
    {
        await _db.StringIncrementAsync(VersionKey);
    }
}

