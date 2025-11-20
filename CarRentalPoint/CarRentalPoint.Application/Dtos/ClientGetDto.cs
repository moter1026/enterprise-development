namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving client details
/// </summary>
/// <param name="Id">Unique identifier of the client</param>
/// <param name="FullName">Full name of the client</param>
/// <param name="DriverLicenseNumber">Driver license number of the client</param>
/// <param name="BirthDate">Birth date of the client</param>
public record ClientGetDto(
    int Id,
    string FullName,
    string DriverLicenseNumber,
    DateOnly BirthDate
);
