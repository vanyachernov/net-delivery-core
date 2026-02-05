var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddConnectionString("DefaultConnection");

var seq = builder.AddSeq("seq");

builder.AddProject<Projects.Workers_Api>("workers-api")
    .WithReference(db)
    .WithReference(seq)
    .WaitFor(seq);

builder.Build().Run();
