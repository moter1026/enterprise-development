using CarRentalPoint.Entities;

namespace CarRentalPoint.Contract;

/// <summary>
/// Represents a car rental agreement between a client and the company
/// Contains information about rental participants, terms, and financial calculations
/// </summary>
public class Rental
{
    /// <summary>
    /// The client who is renting the car
    /// </summary>
    public required Client Client { get; set; }

    /// <summary>
    /// The car being rented
    /// </summary>
    public required Car Car { get; set; }

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
    public bool IsActive => DateTime.Now < RentalStart.AddHours(RentalHours);

    /// <summary>
    /// Total rental cost calculated as hourly rental rate multiplied by number of hours
    /// Returns 0 if the car or its generation are not defined
    /// </summary>
    public decimal TotalCost => Car?.Generation?.RentalCostPerHour * RentalHours ?? 0;

    /// <summary>
    /// Returns a string representation of the rental in format: "Client Full Name - Car License Plate (Start Date)"
    /// </summary>
    /// <returns>String with basic rental information</returns>
    public override string ToString() => $"{Client?.FullName} - {Car?.LicensePlate} ({RentalStart})";
}