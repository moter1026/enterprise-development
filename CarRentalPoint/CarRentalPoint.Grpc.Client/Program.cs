using CarRentalPoint.Api.Grpc;
using CarRentalPoint.Grpc.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(serviceProvider =>
{
    var grpcUrl = Environment.GetEnvironmentVariable("services__CarRentalPointApiGrpc__http__0") ?? 
        throw new InvalidOperationException("Grpc URL not found");
    var channel = Grpc.Net.Client.GrpcChannel.ForAddress(grpcUrl);
    return new RentalReceiver.RentalReceiverClient(channel);
});

var host = builder.Build();

host.Services.GetRequiredService<RentalReceiver.RentalReceiverClient>();

host.Run();
