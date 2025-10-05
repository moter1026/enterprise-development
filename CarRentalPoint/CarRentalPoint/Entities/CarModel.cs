namespace CarRentalPoint.Entities;
// Справочник модели автомобиля
public class CarModel
{
    public required string Name { get; set; }
    public required string DriveType { get; set; }
    public required int SeatsCount { get; set; }
    public required string BodyType { get; set; }
    public required string CarClass { get; set; }
}
