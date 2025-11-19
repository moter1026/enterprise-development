namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving rental details
/// </summary>
public record RentalGetDto(
    /// <summary>
    /// Unique identifier of the rental
    /// </summary>
    int Id,

    /// <summary>
    /// Details of the client who rented the car
    /// </summary>
    ClientGetDto Client,

    /// <summary>
    /// Details of the rented car
    /// </summary>
    CarGetDto Car,

    /// <summary>
    /// Start date and time of the rental
    /// </summary>
    DateTime RentalStart,

    /// <summary>
    /// Duration of the rental in hours
    /// </summary>
    int RentalHours,

    /// <summary>
    /// Indicates whether the rental is currently active
    /// </summary>
    bool IsActive,

    /// <summary>
    /// Total cost of the rental
    /// </summary>
    decimal TotalCost
);
