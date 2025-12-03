using Bogus;
using CarRentalPoint.Api.Grpc;

namespace CarRentalPoint.Grpc.Client;

/// <summary>
/// Utility class responsible for generating random Car, Client and Rental requests.
/// Uses Bogus library for realistic test data.
/// </summary>
public class Generator
{
    /// <summary>
    /// Internal Faker instance used to generate realistic random data
    /// such as names, dates, vehicle information, and numeric values.
    /// </summary>
    private readonly Faker _faker = new();

    /// <summary>
    /// Generates a random rental contract.
    /// </summary>
    /// <param name="maxClientId">Maximum client ID for random selection.</param>
    /// <param name="maxCarId">Maximum car ID for random selection.</param>
    public RentalRequest GenerateRandomRental(int maxClientId = 10, int maxCarId = 10)
    {
        return new RentalRequest
        {
            ClientId = _faker.Random.Int(1, maxClientId),
            CarId = _faker.Random.Int(1, maxCarId),
            RentalStart = _faker.Date.Recent().ToString("O"),
            RentalHours = _faker.Random.Int(1, 72)
        };
    }

    /// <summary>
    /// Generates a random client profile.
    /// </summary>
    public ClientRequest GenerateRandomClient()
    {
        var birthDate = _faker.Date.Past(50, DateTime.Today.AddYears(-18));
        return new ClientRequest
        {
            FullName = _faker.Name.FullName(),
            DriverLicenseNumber = _faker.Random.Replace("??######"),
            BirthDate = birthDate.ToString("yyyy-MM-dd")
        };
    }

    /// <summary>
    /// Generates a random car object.
    /// </summary>
    public CarRequest GenerateRandomCar()
    {
        return new CarRequest
        {
            LicensePlate = _faker.Vehicle.Vin()[..8].ToUpper(),
            Color = _faker.Commerce.Color(),
            GenerationId = _faker.Random.Int(1, 5)
        };
    }
}
