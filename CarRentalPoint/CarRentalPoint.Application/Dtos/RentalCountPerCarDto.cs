namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO representing a car along with its rental count
/// </summary>
/// <param name="Id">Unique identifier of the car</param>
/// <param name="LicensePlate">Car's license plate</param>
/// <param name="Color">Car's color</param>
/// <param name="Generation">Model generation details of the car</param>
/// <param name="RentalCount">Number of times the car has been rented</param>
public record RentalCountPerCarDto(
    int Id,
    string LicensePlate,
    string Color,
    ModelGenerationGetDto Generation,
    int RentalCount
);
