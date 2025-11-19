namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving car details
/// </summary>
public record CarGetDto(
    /// <summary>
    /// Unique identifier of the car
    /// </summary>
    int Id,

    /// <summary>
    /// Car's license plate
    /// </summary>
    string LicensePlate,

    /// <summary>
    /// Car's color
    /// </summary>
    string Color,

    /// <summary>
    /// Model generation details of the car
    /// </summary>
    ModelGenerationGetDto Generation
);