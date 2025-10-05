namespace CarRentalPoint.Entities;

/// <summary>
/// Represents a physical car available for rental
/// Contains identification information and technical specifications through its generation
/// </summary>
public class Car
{
    /// <summary>
    /// Unique license plate number that identifies the vehicle
    /// </summary>
    public required string LicensePlate { get; set; }

    /// <summary>
    /// Exterior color of the car
    /// </summary>
    public required string Color { get; set; }

    /// <summary>
    /// Technical specifications and model information of the car
    /// </summary>
    public required ModelGeneration Generation { get; set; }
}