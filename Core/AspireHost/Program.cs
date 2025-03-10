var builder = DistributedApplication.CreateBuilder(args);

Console.WriteLine(builder.Environment.ToString());

const string scheme = "http";

var service = builder.AddProject<Projects.Api>("api")
    .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development");

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
