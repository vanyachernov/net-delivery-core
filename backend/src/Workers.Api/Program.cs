using Scalar.AspNetCore;
using Workers.Infrastructure;
using Workers.Api.Middlewares;
using Workers.Application;


var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
{
    builder.Services.AddOpenApi();
    //builder.Services.AddApplication().AddInfrastructure(builder.Configuration);
    builder.Services.AddApplication().AddInfrastructure(builder.Configuration);

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