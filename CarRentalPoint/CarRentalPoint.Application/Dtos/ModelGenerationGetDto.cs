namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving car model generation details
/// </summary>
public record ModelGenerationGetDto(
    /// <summary>
    /// Unique identifier of the model generation
    /// </summary>
    int Id,

    /// <summary>
    /// Year of the model generation
    /// </summary>
    int Year,

    /// <summary>
    /// Engine volume in liters
    /// </summary>
    double EngineVolume,

    /// <summary>
    /// Type of transmission (e.g., manual, automatic)
    /// </summary>
    string TransmissionType,

    /// <summary>
    /// Rental cost per hour for this generation
    /// </summary>
    decimal RentalCostPerHour,

    /// <summary>
    /// Details of the associated car model
    /// </summary>
    CarModelGetDto Model
);
