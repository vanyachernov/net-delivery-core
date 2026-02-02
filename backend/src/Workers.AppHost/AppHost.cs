var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddConnectionString("DefaultConnection");

builder.AddProject<Projects.Workers_Api>("workers-api")
    .WithReference(db);

builder.Build().Run();
