namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for updating car details
/// </summary>
/// <param name="LicensePlate">Car's license plate</param>
/// <param name="Color">Car's color</param>
/// <param name="GenerationId">Model generation ID</param>
public record CarEditDto(string LicensePlate, string Color, int GenerationId);
