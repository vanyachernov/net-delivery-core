using Serilog;

namespace Workers.Api.Extensions;

public static class LoggingExtensions
{
    public static void ConfigureLogging(this WebApplicationBuilder builder)
    {
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
    }
}
