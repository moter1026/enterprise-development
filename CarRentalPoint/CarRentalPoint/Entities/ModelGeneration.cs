namespace CarRentalPoint.Entities;
// Справочник поколения модели
public class ModelGeneration
{
    public required int Year { get; set; }
    public required double EngineVolume { get; set; }
    public required string TransmissionType { get; set; }
    public required CarModel Model { get; set; }
    public required decimal RentalCostPerHour { get; set; }
}