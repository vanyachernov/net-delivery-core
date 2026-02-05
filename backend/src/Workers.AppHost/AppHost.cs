var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddConnectionString("DefaultConnection");
var redis = builder.AddRedis("redis");

var seq = builder.AddSeq("seq");

builder.AddProject<Projects.Workers_Api>("workers-api")
    .WithReference(db)
    .WithReference(seq)
    .WithReference(redis)
    .WaitFor(seq)
    .WaitFor(redis);

builder.Build().Run();
