namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing car model details
/// </summary>
public record CarModelEditDto(
    /// <summary>
    /// Name of the car model
    /// </summary>
    string Name,

    /// <summary>
    /// Type of drive (e.g., FWD, RWD, AWD)
    /// </summary>
    string DriveType,

    /// <summary>
    /// Number of seats in the car
    /// </summary>
    int SeatsCount,

    /// <summary>
    /// Type of car body (e.g., sedan, SUV)
    /// </summary>
    string BodyType,

    /// <summary>
    /// Car class category (e.g., economy, luxury)
    /// </summary>
    string CarClass
);
