using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalPoint.Application.Dtos;

/// <summary>
/// DTO for retrieving rental details
/// </summary>
/// <param name="Id">Unique identifier of the rental</param>
/// <param name="Client">Details of the client who rented the car</param>
/// <param name="Car">Details of the rented car</param>
/// <param name="RentalStart">Start date and time of the rental</param>
/// <param name="RentalHours">Duration of the rental in hours</param>
/// <param name="IsActive">Indicates whether the rental is currently active</param>
/// <param name="TotalCost">Total cost of the rental</param>
public record RentalGetDto(
    int Id,
    ClientGetDto Client,
    CarGetDto Car,
    DateTime RentalStart,
    int RentalHours,
    bool IsActive,
    decimal TotalCost
);
