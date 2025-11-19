namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing client details
/// </summary>
public record ClientEditDto(
    /// <summary>
    /// Full name of the client
    /// </summary>
    string FullName,

    /// <summary>
    /// Driver license number of the client
    /// </summary>
    string DriverLicenseNumber,

    /// <summary>
    /// Birth date of the client
    /// </summary>
    DateTime BirthDate
);
