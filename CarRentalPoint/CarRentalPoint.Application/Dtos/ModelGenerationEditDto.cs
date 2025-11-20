namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing car model generation details
/// </summary>
/// <param name="Year">Year of the model generation</param>
/// <param name="EngineVolume">Engine volume in liters</param>
/// <param name="TransmissionType">Type of transmission</param>
/// <param name="RentalCostPerHour">Rental cost per hour for this generation</param>
/// <param name="ModelId">Identifier of the associated car model</param>
public record ModelGenerationEditDto(
    int Year,
    double EngineVolume,
    string TransmissionType,
    decimal RentalCostPerHour,
    int ModelId
);
