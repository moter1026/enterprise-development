namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing car model details
/// </summary>
/// <param name="Name">Name of the car model</param>
/// <param name="DriveType">Type of drive</param>
/// <param name="SeatsCount">Number of seats in the car</param>
/// <param name="BodyType">Type of car body</param>
/// <param name="CarClass">Car class category</param>
public record CarModelEditDto(
    string Name,
    string DriveType,
    int SeatsCount,
    string BodyType,
    string CarClass
);
