namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving car model generation details
/// </summary>
/// <param name="Id">Unique identifier of the model generation</param>
/// <param name="Year">Year of the model generation</param>
/// <param name="EngineVolume">Engine volume in liters</param>
/// <param name="TransmissionType">Type of transmission</param>
/// <param name="RentalCostPerHour">Rental cost per hour for this generation</param>
/// <param name="Model">Details of the associated car model</param>
public record ModelGenerationGetDto(
    int Id,
    int Year,
    double EngineVolume,
    string TransmissionType,
    decimal RentalCostPerHour,
    CarModelGetDto Model
);
