using CarRentalPoint.Api.Grpc;
using CarRentalPoint.Grpc.Client;
using Grpc.Net.Client;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddSingleton(serviceProvider =>
{
    var apiGrpcUrl = builder.Configuration["ApiGrpcUrl"] ??
        throw new InvalidOperationException("ApiGrpcUrl not found");
    var channel = GrpcChannel.ForAddress(apiGrpcUrl);
    return new RentalReceiver.RentalReceiverClient(channel);
});

var host = builder.Build();

host.Services.GetRequiredService<RentalReceiver.RentalReceiverClient>();

host.Run();
