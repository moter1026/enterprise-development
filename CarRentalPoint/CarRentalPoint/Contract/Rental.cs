using CarRentalPoint.Entities;

namespace CarRentalPoint.Contract;
// Аренда (контракт)
public class Rental
{
    public required Client Client { get; set; }
    public required Car Car { get; set; }
    public required DateTime RentalStart { get; set; }
    public required int RentalHours { get; set; }
    public bool IsActive => DateTime.Now < RentalStart.AddHours(RentalHours);

    public decimal TotalCost => Car?.Generation?.RentalCostPerHour * RentalHours ?? 0;

    public override string ToString() => $"{Client?.FullName} - {Car?.LicensePlate} ({RentalStart})";
}