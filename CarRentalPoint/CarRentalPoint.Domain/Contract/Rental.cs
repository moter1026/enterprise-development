using CarRentalPoint.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalPoint.Domain.Contract;

/// <summary>
/// Represents a car rental agreement between a client and the company
/// Contains information about rental participants, terms, and financial calculations
/// </summary>
public class Rental
{
    /// <summary>
    /// Unique identifier for the car rental agreement
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Unique identifier for the client that rented the car
    /// </summary>
    public required int ClientId { get; set; }

    /// <summary>
    /// The client who is renting the car
    /// </summary>
    public Client? Client { get; set; }

    /// <summary>
    /// Unique identifier for the rented car
    /// </summary>
    public required int CarId { get; set; }

    /// <summary>
    /// The car being rented
    /// </summary>
    public Car? Car { get; set; }

    /// <summary>
    /// Date and time when the rental starts
    /// </summary>
    public required DateTime RentalStart { get; set; }

    /// <summary>
    /// Rental duration in hours
    /// </summary>
    public required int RentalHours { get; set; }

    /// <summary>
    /// Indicates whether the rental is currently active
    /// Returns true if current time is before the rental end date
    /// </summary>
    [NotMapped]
    public bool IsActive => DateTime.Now < RentalStart.AddHours(RentalHours);

    /// <summary>
    /// Total rental cost calculated as hourly rental rate multiplied by number of hours
    /// Returns 0 if the car or its generation are not defined
    /// </summary>
    public decimal TotalCost => (Car?.Generation?.RentalCostPerHour ?? 0) * RentalHours;
}