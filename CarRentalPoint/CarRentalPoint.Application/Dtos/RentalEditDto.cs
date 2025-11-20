namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for editing rental details
/// </summary>
/// <param name="ClientId">Identifier of the client renting the car</param>
/// <param name="CarId">Identifier of the rented car</param>
/// <param name="RentalStart">Start date and time of the rental</param>
/// <param name="RentalHours">Duration of the rental in hours</param>
public record RentalEditDto(
    int ClientId,
    int CarId,
    DateTime RentalStart,
    int RentalHours
);
