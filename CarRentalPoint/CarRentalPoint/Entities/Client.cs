namespace CarRentalPoint.Entities;
// Клиент
public class Client
{
    public string DriverLicenseNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    public override string ToString() => FullName;
}
