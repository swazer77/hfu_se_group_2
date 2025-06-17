var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Blazor_Dashboard_ApiService>("apiservice");

builder.AddProject<Projects.Blazor_Dashboard_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
