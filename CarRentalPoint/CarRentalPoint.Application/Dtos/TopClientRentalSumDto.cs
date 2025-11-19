namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO representing a client along with their total rental cost
/// </summary>
public record TopClientRentalSumDto(
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
    DateTime BirthDate,

    /// <summary>
    /// Total rental cost accumulated by the client
    /// </summary>
    decimal TotalRentalCost
);
