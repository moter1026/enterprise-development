namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO representing a car along with its rental count
/// </summary>
public record RentalCountPerCarDto(
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
    ModelGenerationGetDto Generation,

    /// <summary>
    /// Number of times the car has been rented
    /// </summary>
    int RentalCount
);
