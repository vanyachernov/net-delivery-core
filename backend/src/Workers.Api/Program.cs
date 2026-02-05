using Serilog;
using Scalar.AspNetCore;
using Workers.Infrastructure;
using Workers.Api.Middlewares;
using Workers.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.Seq(
            serverUrl: context.Configuration["Seq:ServerUrl"] ?? "http://localhost:5341")
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
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddOpenApi();
    builder.Services.AddApplication();
    builder.AddInfrastructure();
    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();
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