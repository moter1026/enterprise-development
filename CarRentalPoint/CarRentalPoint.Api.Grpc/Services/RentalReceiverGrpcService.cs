using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Options;

namespace CarRentalPoint.Api.Grpc.Services;

/// <summary>
/// gRPC service for receiving rentals, clients, and cars via streaming.
/// Processes incoming streams in batches and writes responses for each item.
/// </summary>
/// <param name="rentalRepo">Repository for accessing and storing <see cref="Rental"/> entities.</param>
/// <param name="clientRepo">Repository for accessing and storing <see cref="Client"/> entities.</param>
/// <param name="carRepo">Repository for accessing and storing <see cref="Car"/> entities.</param>
/// <param name="logger">Logger instance for recording service activity.</param>
/// <param name="optionsAccessor">Accessor for runtime options controlling batch size and payload limits.</param>
public class RentalReceiverGrpcService(
    IRepository<Rental> rentalRepo,
    IRepository<Client> clientRepo,
    IRepository<Car> carRepo,
    ILogger<RentalReceiverGrpcService> logger,
    IOptions<RentalReceiverOptions> optionsAccessor)
    : RentalReceiver.RentalReceiverBase
{
    /// <summary>
    /// Runtime configuration options controlling batch size, payload limits,
    /// and gRPC message size limits.
    /// </summary>
    private readonly RentalReceiverOptions _options = optionsAccessor.Value;

    /// <summary>
    /// Validates the size of the incoming gRPC message against the configured payload limit.
    /// </summary>
    /// <param name="msg">The gRPC message to validate.</param>
    /// <param name="error">Error message if validation fails.</param>
    /// <returns>True if payload is within limits; otherwise false.</returns>
    private bool ValidatePayload(IMessage msg, out string? error)
    {
        if (msg.CalculateSize() > _options.PayloadLimitBytes)
        {
            error = "Payload too large";
            return false;
        }
        error = null;
        return true;
    }

    /// <summary>
    /// Saves a batch of entities to the repository and logs the operation.
    /// </summary>
    /// <typeparam name="T">Entity type.</typeparam>
    /// <param name="batch">List of entities to save.</param>
    /// <param name="repo">Repository to save into.</param>
    /// <param name="name">Name of the entity type for logging.</param>
    private async Task SaveBatchAsync<T>(List<T> batch, IRepository<T> repo, string name)
        where T : class
    {
        foreach (var item in batch)
            await repo.AddAsync(item);

        logger.LogInformation("Saved batch of {Count} {Type}", batch.Count, name);
        batch.Clear();
    }

    /// <summary>
    /// Receives a stream of rental requests, validates them, stores in batches,
    /// and sends a response for each request.
    /// </summary>
    /// <param name="requestStream">Incoming rental stream.</param>
    /// <param name="responseStream">Outgoing response stream.</param>
    /// <param name="context">gRPC call context.</param>
    public override async Task StreamRentals(
        IAsyncStreamReader<RentalRequest> requestStream,
        IServerStreamWriter<RentalResponse> responseStream,
        ServerCallContext context)
    {
        var batch = new List<Rental>();

        await foreach (var req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (!ValidatePayload(req, out var sizeError))
            {
                await responseStream.WriteAsync(new RentalResponse { Success = false, Error = sizeError });
                continue;
            }

            logger.LogInformation("Rental received: {@Req}", req);

            if (req.ClientId <= 0 || req.CarId <= 0 || req.RentalHours <= 0)
            {
                await responseStream.WriteAsync(new RentalResponse
                {
                    Success = false,
                    Error = "ClientId, CarId, RentalHours must be positive."
                });
                continue;
            }

            if (!DateTime.TryParse(req.RentalStart, out var rentalStart))
            {
                await responseStream.WriteAsync(new RentalResponse
                {
                    Success = false,
                    Error = $"Invalid date: {req.RentalStart}"
                });
                continue;
            }

            var client = await clientRepo.GetByIdAsync(req.ClientId);
            var car = await carRepo.GetByIdAsync(req.CarId);

            if (client == null || car == null)
            {
                await responseStream.WriteAsync(new RentalResponse
                {
                    Success = false,
                    Error = "Client or car not found."
                });
                continue;
            }

            batch.Add(new Rental
            {
                ClientId = req.ClientId,
                CarId = req.CarId,
                RentalHours = req.RentalHours,
                RentalStart = rentalStart.ToUniversalTime()
            });

            await responseStream.WriteAsync(new RentalResponse { Success = true });

            if (batch.Count >= _options.BatchSize)
                await SaveBatchAsync(batch, rentalRepo, "rentals");
        }

        if (batch.Count > 0)
            await SaveBatchAsync(batch, rentalRepo, "rentals");
    }

    /// <summary>
    /// Receives a stream of client requests, validates them, stores in batches,
    /// and sends a response for each request.
    /// </summary>
    /// <param name="requestStream">Incoming client stream.</param>
    /// <param name="responseStream">Outgoing response stream.</param>
    /// <param name="context">gRPC call context.</param>
    public override async Task StreamClients(
        IAsyncStreamReader<ClientRequest> requestStream,
        IServerStreamWriter<ClientResponse> responseStream,
        ServerCallContext context)
    {
        var batch = new List<Client>();

        await foreach (var req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (!ValidatePayload(req, out var sizeError))
            {
                await responseStream.WriteAsync(new ClientResponse { Success = false, Error = sizeError });
                continue;
            }

            logger.LogInformation("Client received: {@Req}", req);

            if (string.IsNullOrWhiteSpace(req.FullName) ||
                string.IsNullOrWhiteSpace(req.DriverLicenseNumber) ||
                !DateOnly.TryParse(req.BirthDate, out var birthDate))
            {
                await responseStream.WriteAsync(new ClientResponse
                {
                    Success = false,
                    Error = "Invalid client data."
                });
                continue;
            }

            batch.Add(new Client
            {
                FullName = req.FullName,
                DriverLicenseNumber = req.DriverLicenseNumber,
                BirthDate = birthDate
            });

            await responseStream.WriteAsync(new ClientResponse { Success = true });

            if (batch.Count >= _options.BatchSize)
                await SaveBatchAsync(batch, clientRepo, "clients");
        }

        if (batch.Count > 0)
            await SaveBatchAsync(batch, clientRepo, "clients");
    }

    /// <summary>
    /// Receives a stream of car requests, validates them, stores in batches,
    /// and sends a response for each request.
    /// </summary>
    /// <param name="requestStream">Incoming car stream.</param>
    /// <param name="responseStream">Outgoing response stream.</param>
    /// <param name="context">gRPC call context.</param>
    public override async Task StreamCars(
        IAsyncStreamReader<CarRequest> requestStream,
        IServerStreamWriter<CarResponse> responseStream,
        ServerCallContext context)
    {
        var batch = new List<Car>();

        await foreach (var req in requestStream.ReadAllAsync(context.CancellationToken))
        {
            if (!ValidatePayload(req, out var sizeError))
            {
                await responseStream.WriteAsync(new CarResponse { Success = false, Error = sizeError });
                continue;
            }

            logger.LogInformation("Car received: {@Req}", req);

            if (string.IsNullOrWhiteSpace(req.LicensePlate) ||
                string.IsNullOrWhiteSpace(req.Color) ||
                req.GenerationId <= 0)
            {
                await responseStream.WriteAsync(new CarResponse
                {
                    Success = false,
                    Error = "Invalid car data."
                });
                continue;
            }

            batch.Add(new Car
            {
                LicensePlate = req.LicensePlate,
                Color = req.Color,
                ModelGenerationId = req.GenerationId
            });

            await responseStream.WriteAsync(new CarResponse { Success = true });

            if (batch.Count >= _options.BatchSize)
                await SaveBatchAsync(batch, carRepo, "cars");
        }

        if (batch.Count > 0)
            await SaveBatchAsync(batch, carRepo, "cars");
    }
}