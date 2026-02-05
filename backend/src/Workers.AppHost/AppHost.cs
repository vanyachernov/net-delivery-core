var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddConnectionString("DefaultConnection");
var redis = builder.AddRedis("redis");

builder.AddProject<Projects.Workers_Api>("workers-api")
    .WithReference(db)
    .WithReference(redis);

builder.Build().Run();
