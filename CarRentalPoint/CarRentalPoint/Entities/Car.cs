namespace CarRentalPoint.Entities;

// Класс автомобиля

public class Car
{
    public required string LicensePlate { get; set; }
    public required string Color { get; set; }
    public required ModelGeneration Generation { get; set; }
}
