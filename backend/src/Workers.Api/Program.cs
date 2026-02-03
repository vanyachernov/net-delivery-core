using Serilog;
using Scalar.AspNetCore;
using Workers.Infrastructure;
using Workers.Infrastructure.Persistence;
using Workers.Api.Models;
using Workers.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.OpenTelemetry(options =>
        {
            options.Endpoint = context.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"];
            options.ResourceAttributes = new Dictionary<string, object>
            {
                ["service.name"] = "workers-api"
            };
        });
});

builder.AddServiceDefaults();
{
    builder.Services.AddOpenApi();
    builder.AddInfrastructure();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
    builder.Services.AddControllers();
}

var app = builder.Build();

app.MapDefaultEndpoints();
{
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseExceptionHandler();
    app.UseHttpsRedirection();
    app.MapControllers();
    app.Run();
}