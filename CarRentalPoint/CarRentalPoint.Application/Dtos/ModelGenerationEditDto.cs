namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing car model generation details
/// </summary>
public record ModelGenerationEditDto(
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
    /// Identifier of the associated car model
    /// </summary>
    int ModelId
);