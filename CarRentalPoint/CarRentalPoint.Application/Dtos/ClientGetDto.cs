namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving client details
/// </summary>
public record ClientGetDto(
    /// <summary>
    /// Unique identifier of the client
    /// </summary>
    int Id,

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
