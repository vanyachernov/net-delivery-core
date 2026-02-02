using Scalar.AspNetCore;
using Workers.Infrastructure;
using Workers.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
{
    builder.Services.AddOpenApi();
    builder.Services.AddInfrastructure(builder.Configuration);
}

var app = builder.Build();

app.MapDefaultEndpoints();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.MapGet("/health-db", async (ApplicationDbContext context) =>
    {
        var canConnect = await context.Database.CanConnectAsync();
        return new { Status = "Healthy", DatabaseConnected = canConnect };
    });

    app.UseHttpsRedirection();
    app.Run();
}