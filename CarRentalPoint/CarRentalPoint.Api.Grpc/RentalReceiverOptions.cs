namespace CarRentalPoint.Api.Grpc;

/// <summary>
/// Configuration options used by <see cref="RentalReceiverGrpcService"/> to control
/// batching behavior, payload validation, and gRPC message size limits.
/// </summary>
public class RentalReceiverOptions
{
    /// <summary>
    /// Gets or sets the number of rental records to accumulate before writing
    /// them to the repository in a single batch operation.
    /// </summary>
    public int BatchSize { get; set; } = 50;

    /// <summary>
    /// Gets or sets the maximum allowed serialized payload size (in bytes)
    /// for a single <see cref="RentalRequest"/> received over the streaming endpoint.
    /// Requests exceeding this limit are rejected with an error response.
    /// </summary>
    public int PayloadLimitBytes { get; set; } = 5 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the maximum allowed size (in bytes) of inbound gRPC messages.
    /// This limit is enforced by the gRPC channel and protects the server
    /// from excessively large incoming messages.
    /// </summary>
    public int MaxReceiveMessageSizeBytes { get; set; } = 10 * 1024 * 1024;

    /// <summary>
    /// Gets or sets the maximum allowed size (in bytes) of outbound gRPC messages.
    /// This prevents the server from generating responses larger than
    /// the gRPC channel is configured to handle.
    /// </summary>
    public int MaxSendMessageSizeBytes { get; set; } = 10 * 1024 * 1024;
}
