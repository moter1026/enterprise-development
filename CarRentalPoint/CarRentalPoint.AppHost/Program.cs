var builder = DistributedApplication.CreateBuilder(args);

var rentalConfig = builder.Configuration.GetSection("RentalReceiver");
var batchSize = rentalConfig["BatchSize"] ?? "50";
var payloadLimit = rentalConfig["PayloadLimitBytes"] ?? "5242880";
var maxReceiveSize = rentalConfig["MaxReceiveMessageSizeBytes"] ?? "10485760";
var maxSendSize = rentalConfig["MaxSendMessageSizeBytes"] ?? "10485760";
var workerDelay = builder.Configuration["Worker:DelaySeconds"] ?? "1";

var batchSizeParam = builder.AddParameter("batchSize", batchSize);
var payloadLimitParam = builder.AddParameter("payloadLimit", payloadLimit);
var maxReceiveSizeParam = builder.AddParameter("maxReceiveSize", maxReceiveSize);
var maxSendSizeParam = builder.AddParameter("maxSendSize", maxSendSize);
var workerDelayParam = builder.AddParameter("workerDelay", workerDelay);

var postgres = builder.AddPostgres("PostgreSQL");

var postgresDb = postgres.AddDatabase("CarRentalPointDB");

var api = builder.AddProject<Projects.CarRentalPoint_Api>("CarRentalPointApi")
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

var grpcReceiver = builder.AddProject<Projects.CarRentalPoint_Api_Grpc>("CarRentalPointApiGrpc")
    .WithEnvironment("RentalReceiver__BatchSize", batchSizeParam)
    .WithEnvironment("RentalReceiver__PayloadLimitBytes", payloadLimitParam)
    .WithEnvironment("RentalReceiver__MaxReceiveMessageSizeBytes", maxReceiveSizeParam)
    .WithEnvironment("RentalReceiver__MaxSendMessageSizeBytes", maxSendSizeParam)
    .WithReference(postgresDb, "DefaultConnection")
    .WaitFor(postgresDb);

builder.AddProject<Projects.CarRentalPoint_Grpc_Client>("CarRentalPointGrpcClient")
    .WithEnvironment("WORKER_DELAY_SECONDS", workerDelayParam)
    .WithReference(grpcReceiver)
    .WaitFor(grpcReceiver)
    .WithExplicitStart();

builder.Build().Run();
