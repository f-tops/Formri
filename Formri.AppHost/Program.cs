var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.Formri_Api>("apiservice");

builder.AddProject<Projects.Formri_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
