namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for updating car details
/// </summary>
public record CarEditDto(
    /// <summary>
    /// Car's license plate
    /// </summary>
    string LicensePlate,

    /// <summary>
    /// Car's color
    /// </summary>
    string Color,

    /// <summary>
    /// Model generation ID
    /// </summary>
    int GenerationId
);