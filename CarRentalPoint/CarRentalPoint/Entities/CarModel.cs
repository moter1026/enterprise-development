using CarRentalPoint.Enums;

namespace CarRentalPoint.Entities;

/// <summary>
/// Reference data for car models
/// Contains classification and specification information for vehicle models
/// </summary>
public class CarModel
{
    /// <summary>
    /// Unique identifier for the model
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the car model (e.g., "Toyota Camry", "BMW X5")
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Type of drive system (e.g., "Front-wheel", "All-wheel", "Rear-wheel")
    /// </summary>
    public required CarDriveType DriveType { get; set; }

    /// <summary>
    /// Number of passenger seats in the vehicle
    /// </summary>
    public required int SeatsCount { get; set; }

    /// <summary>
    /// Body type of the vehicle (e.g., "Sedan", "SUV", "Hatchback")
    /// </summary>
    public required CarBodyType BodyType { get; set; }

    /// <summary>
    /// Vehicle classification category (e.g., "A", "B", "C", "D", "E")
    /// </summary>
    public required CarClass CarClass { get; set; }
}