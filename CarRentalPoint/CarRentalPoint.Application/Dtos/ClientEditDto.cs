namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing client details
/// </summary>
/// <param name="FullName">Full name of the client</param>
/// <param name="DriverLicenseNumber">Driver license number of the client</param>
/// <param name="BirthDate">Birth date of the client</param>
public record ClientEditDto(
    string FullName,
    string DriverLicenseNumber,
    DateOnly BirthDate
);
