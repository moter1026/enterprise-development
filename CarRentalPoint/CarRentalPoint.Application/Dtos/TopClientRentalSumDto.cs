namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO representing a client along with their total rental cost
/// </summary>
/// <param name="Id">Unique identifier of the client</param>
/// <param name="FullName">Full name of the client</param>
/// <param name="DriverLicenseNumber">Driver license number of the client</param>
/// <param name="BirthDate">Birth date of the client</param>
/// <param name="TotalRentalCost">Total rental cost accumulated by the client</param>
public record TopClientRentalSumDto(
    int Id,
    string FullName,
    string DriverLicenseNumber,
    DateOnly BirthDate,
    decimal TotalRentalCost
);
