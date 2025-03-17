var builder = DistributedApplication.CreateBuilder(args);

Console.WriteLine(builder.Environment.ToString());

const string scheme = "http";

var seq = builder.AddContainer("seq", "datalust/seq")
    .WithEnvironment("ACCEPT_EULA", "Y")
    .WithVolume("seq-data", "/data")
        .WithEndpoint(
        port: 5341,
        targetPort: 80,
        name: "seq-endpoint",
        scheme: "http"
    );

var postgres = builder.AddContainer("postgres", "postgres:17")
    .WithEnvironment("POSTGRES_USER", "TestUser")
    .WithEnvironment("POSTGRES_PASSWORD", "1234")
    .WithEnvironment("POSTGRES_DB", "TestDb")
    .WithVolume("postgres-data", "/var/lib/postgresql/data") 
    .WithEndpoint(port: 5432, targetPort: 5432, name: "postgres-endpoint") 
    .WaitFor(seq); 

var service = builder.AddProject<Projects.Api>("api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
    .WaitFor(postgres);

var sender = builder.AddNpmApp("client-sender", "../../UserInterfaces/client-sender")
    .WithEndpoint(port: 60717, scheme: scheme, env: "VITE_PORT")
    .WithReference(service)
    .WaitFor(service)
    .PublishAsDockerFile();

builder.AddNpmApp("client-websocket", "../../UserInterfaces/client-websocket")
    .WithEndpoint(port: 60718, scheme: scheme, env: "VITE_PORT")
    .WithReference(service)
    .WaitFor(sender)
    .PublishAsDockerFile();

builder.AddNpmApp("client-recent-messages", "../../UserInterfaces/client-recent-messages")
    .WithEndpoint(port: 60719, scheme: scheme, env: "VITE_PORT")
    .WithReference(service)
    .WaitFor(sender)
    .PublishAsDockerFile();

builder.Build().Run();
