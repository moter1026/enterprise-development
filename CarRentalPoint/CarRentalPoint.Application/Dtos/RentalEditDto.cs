namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing rental details
/// </summary>
public record RentalEditDto(
    /// <summary>
    /// Identifier of the client renting the car
    /// </summary>
    int ClientId,

    /// <summary>
    /// Identifier of the rented car
    /// </summary>
    int CarId,

    /// <summary>
    /// Start date and time of the rental
    /// </summary>
    DateTime RentalStart,

    /// <summary>
    /// Duration of the rental in hours
    /// </summary>
    int RentalHours
);
