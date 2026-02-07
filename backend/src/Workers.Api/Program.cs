using Workers.Infrastructure;
using Workers.Api.Middlewares;
using Workers.Application;
using Workers.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure Logging
builder.ConfigureLogging();

builder.AddServiceDefaults();

// Add Services
builder.Services.AddControllers();
builder.Services.ConfigureVersioning();
builder.Services.ConfigureOpenApi();
builder.Services.AddApplication();
builder.AddInfrastructure();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Configure Auth
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure Pipeline
app.UseOpenApi();

app.UseExceptionHandler();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Seed Data
await app.SeedRolesAsync();

app.Run();