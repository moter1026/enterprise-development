namespace CarRentalPoint.Domain.Entities;

/// <summary>
/// Reference data for model generations
/// Contains technical specifications and pricing information for specific model years
/// </summary>
public class ModelGeneration
{
    /// <summary>
    /// Unique identifier for the Model Generation
    /// </summary>
    public int Id { get; set; }

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
    /// Unique identifier for the car model
    /// </summary>
    public required int CarModelId { get; set; }

    /// <summary>
    /// Reference to the car model that this generation belongs to
    /// </summary>
    public CarModel? Model { get; set; }

    /// <summary>
    /// Hourly rental cost for vehicles of this model generation
    /// </summary>
    public required decimal RentalCostPerHour { get; set; }
}