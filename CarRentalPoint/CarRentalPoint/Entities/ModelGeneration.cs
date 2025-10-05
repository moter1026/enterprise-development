namespace CarRentalPoint.Entities;

/// <summary>
/// Reference data for model generations
/// Contains technical specifications and pricing information for specific model years
/// </summary>
public class ModelGeneration
{
    /// <summary>
    /// Manufacturing year of this specific model generation
    /// </summary>
    public required int Year { get; set; }

    /// <summary>
    /// Engine displacement volume in liters
    /// </summary>
    public required double EngineVolume { get; set; }

    /// <summary>
    /// Type of transmission (e.g., "Automatic", "Manual")
    /// </summary>
    public required string TransmissionType { get; set; }

    /// <summary>
    /// Reference to the car model that this generation belongs to
    /// </summary>
    public required CarModel Model { get; set; }

    /// <summary>
    /// Hourly rental cost for vehicles of this model generation
    /// </summary>
    public required decimal RentalCostPerHour { get; set; }
}