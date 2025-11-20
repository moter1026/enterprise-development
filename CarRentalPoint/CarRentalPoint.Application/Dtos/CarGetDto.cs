namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving car details
/// </summary>
/// <param name="Id">Unique identifier of the car</param>
/// <param name="LicensePlate">Car's license plate</param>
/// <param name="Color">Car's color</param>
/// <param name="Generation">Model generation details of the car</param>
public record CarGetDto(
    int Id,
    string LicensePlate,
    string Color,
    ModelGenerationGetDto Generation
);
