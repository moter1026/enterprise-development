var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("PostgreSQL");

var postgresDb = postgres.AddDatabase("CarRentalPointDB");

builder.AddProject<Projects.CarRentalPoint_Api>("CarRentalPointApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.Build().Run();
