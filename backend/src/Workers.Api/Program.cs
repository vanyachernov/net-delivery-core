using Serilog;
using Scalar.AspNetCore;
using Workers.Infrastructure;
using Workers.Api.Middlewares;
using Workers.Application;
using Workers.Domain.Events;
using Workers.Infrastructure.Messaging.Consumers;

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
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddOpenApi();
builder.Services.AddApplication();
builder.AddInfrastructure();

builder.Services.AddKafkaConsumer<UserCreatedEventConsumerExample, UserCreatedEvent>(
    groupId: "user-created-consumer-group",
    topics: new[] { "user-events" }
);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddControllers();


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