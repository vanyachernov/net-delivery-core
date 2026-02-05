var builder = DistributedApplication.CreateBuilder(args);

// Kafka Message Broker
var kafka = builder.AddKafka("kafka")
    .WithKafkaUI();

// Workers API
// PostgreSQL connection string берётся из user secrets
builder.AddProject<Projects.Workers_Api>("workers-api")
    .WithReference(kafka);

builder.Build().Run();
