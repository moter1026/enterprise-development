using CarRentalPoint.Api.Grpc;
using Grpc.Core;

namespace CarRentalPoint.Grpc.Client;

/// <summary>
/// Background worker responsible for periodically generating and sending
/// random rentals, clients, or cars to the gRPC server.
/// </summary>
/// <param name="client">The gRPC client used to communicate with the server.</param>
/// <param name="logger">The logger used for structured logging.</param>
/// <param name="config">The configuration containing worker settings.</param>
public class Worker(
    RentalReceiver.RentalReceiverClient client,
    ILogger<Worker> logger,
    IConfiguration config) : BackgroundService
{
    /// <summary>
    /// Utility for generating random rental, client, and car requests.
    /// </summary>
    private readonly Generator _generator = new();

    /// <summary>
    /// Delay between each generated request, configurable via
    /// the WorkerDelaySeconds environment variable.
    /// </summary>
    private readonly TimeSpan _delay = TimeSpan.FromSeconds(
        config.GetValue<int?>("WorkerDelaySeconds") ?? 1);

    /// <summary>
    /// Random instance used to pick which type of message will be sent.
    /// </summary>
    private readonly Random _random = new();

    /// <summary>
    /// Continuously generates and sends random rentals, clients, and cars
    /// to the server via streaming, and logs the responses until cancellation.
    /// </summary>
    /// <param name="stoppingToken">Token to signal cancellation of the worker.</param>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var rentalStream = client.StreamRentals(cancellationToken: stoppingToken);
        using var clientStream = client.StreamClients(cancellationToken: stoppingToken);
        using var carStream = client.StreamCars(cancellationToken: stoppingToken);

        var sendTask = Task.Run(async () =>
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var choice = _random.Next(3);

                switch (choice)
                {
                    case 0:
                        var rental = _generator.GenerateRandomRental();
                        logger.LogInformation("Sending rental request: {@Rental}", rental);
                        await rentalStream.RequestStream.WriteAsync(rental);
                        break;

                    case 1:
                        var clientRequest = _generator.GenerateRandomClient();
                        logger.LogInformation("Sending client request: {@Client}", clientRequest);
                        await clientStream.RequestStream.WriteAsync(clientRequest);
                        break;

                    case 2:
                        var carRequest = _generator.GenerateRandomCar();
                        logger.LogInformation("Sending car request: {@Car}", carRequest);
                        await carStream.RequestStream.WriteAsync(carRequest);
                        break;
                }

                await Task.Delay(_delay, stoppingToken);
            }

            await rentalStream.RequestStream.CompleteAsync();
            await clientStream.RequestStream.CompleteAsync();
            await carStream.RequestStream.CompleteAsync();

        }, stoppingToken);

        var receiveTask = Task.WhenAll(
            Task.Run(async () =>
            {
                await foreach (var response in rentalStream.ResponseStream.ReadAllAsync(stoppingToken))
                    logger.LogInformation("Received rental response: {@Response}", response);
            }, stoppingToken),

            Task.Run(async () =>
            {
                await foreach (var response in clientStream.ResponseStream.ReadAllAsync(stoppingToken))
                    logger.LogInformation("Received client response: {@Response}", response);
            }, stoppingToken),

            Task.Run(async () =>
            {
                await foreach (var response in carStream.ResponseStream.ReadAllAsync(stoppingToken))
                    logger.LogInformation("Received car response: {@Response}", response);
            }, stoppingToken)
        );

        await sendTask;
        await receiveTask;
    }
}
