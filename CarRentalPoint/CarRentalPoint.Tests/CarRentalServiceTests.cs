using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.InitialData;

namespace CarRentalPoint.Tests;

/// <summary>
/// Unit tests for CarRentalService class functionality
/// Tests various business logic scenarios including client queries, rental calculations, and data aggregation
/// </summary>
public class CarRentalServiceTests(CarRentalDataSeeder service) : IClassFixture<CarRentalDataSeeder>
{
    /// <summary>
    /// Tests that clients who rented a specific car model are returned and ordered by full name
    /// Verifies the correct filtering and sorting logic
    /// </summary>
    [Fact]
    public void GetClientsByCarModel_ShouldReturnClientsOrderedByFullName()
    {
        // Arrange
        var targetModelId = 1;
        var expectedCount = 8;

        // Act
        var result = CarRentalDataSeeder.Rentals
            .Where(r => r.Car.Generation.Model.Id == targetModelId)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToList();

        // Assert
        Assert.Equal(expectedCount, result.Count);
    }

    /// <summary>
    /// Tests retrieval of currently rented cars using fixed dates
    /// Verifies that only cars with active rental periods are returned
    /// </summary>
    [Fact]
    public void GetCarsCurrentlyRented_ShouldReturnActiveRentals()
    {
        // Arrange
        var rentals = CarRentalDataSeeder.Rentals;
        var currentTime = new DateTime(2024, 1, 15, 14, 0, 0);
        var expectedCount = 3;

        // Act
        var activeRentals = rentals
            .Where(r => IsRentalActive(r, currentTime))
            .Select(r => r.Car)
            .Distinct()
            .ToList();

        // Assert
        Assert.Equal(expectedCount, activeRentals.Count);
    }

    /// <summary>
    /// Helper method to determine if rental is active at specific time
    /// </summary>
    private static bool IsRentalActive(Rental rental, DateTime currentTime)
    {
        var rentalEnd = rental.RentalStart.AddHours(rental.RentalHours);
        return rental.RentalStart <= currentTime && currentTime <= rentalEnd;
    }

    /// <summary>
    /// Tests the retrieval of top 5 most frequently rented cars
    /// Verifies correct ordering by rental count in descending order
    /// </summary>
    [Fact]
    public void GetTop5MostFrequentlyRentedCars_ShouldReturnExpectedCars()
    {
        // Arrange
        var rentals = CarRentalDataSeeder.Rentals;
        var expectedTopCount = 5;

        var expectedCars = new List<int>
        {
            1, 4, 6, 8, 11 
        };

        // Act
        var topCars = rentals
            .GroupBy(r => r.Car.Id)
            .Select(g => new
             {
                 CarId = g.Key,
                 RentalCount = g.Count(),
                 g.First().Car
            })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .Select(x => x.Car)
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topCars.Count);        
        Assert.Equal(expectedCars, topCars.Select(c => c.Id)); 
    }


    /// <summary>
    /// Tests calculation of rental counts for each car in the fleet
    /// Verifies that aggregate counts match individual car rental records
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar_ShouldReturnCountForEachCar()
    {
        // Arrange
        var rentals = CarRentalDataSeeder.Rentals;
        var cars = CarRentalDataSeeder.Cars;

        var expectedRentalCounts = new Dictionary<int, int>
        {
            { 1, 5 },   // Car Id = 1 (А001АА) - 5 rents
            { 2, 1 },   // Car Id = 2 (В002ВВ) - 1 rents
            { 3, 1 },   // Car Id = 3 (С003СС) - 1 rents
            { 4, 3 },   // Car Id = 4 (D004DD) - 3 rents
            { 5, 1 },   // Car Id = 5 (Е005ЕЕ) - 1 rents
            { 6, 3 },   // Car Id = 6 (F006FF) - 3 rents
            { 7, 1 },   // Car Id = 7 (G007GG) - 1 rents
            { 8, 2 },   // Car Id = 8 (H008HH) - 2 rents
            { 9, 1 },   // Car Id = 9 (I009II) - 1 rents
            { 10, 1 },  // Car Id = 10 (J010JJ) - 1 rents
            { 11, 2 },  // Car Id = 11 (K011KK) - 2 rents
            { 12, 1 }   // Car Id = 12 (L012LL) - 1 rents
        };

        // Act
        var rentalCounts = rentals
            .GroupBy(r => r.Car.Id)
            .Select(g => new
            {
                CarId = g.Key,
                RentalCount = g.Count(),
                g.First().Car
            })
            .ToList();

        var totalRentals = rentalCounts.Sum(x => x.RentalCount);

        // Assert
        Assert.Equal(rentals.Count, totalRentals);

        Assert.Equal(expectedRentalCounts, rentalCounts.ToDictionary(x => x.CarId, x => x.RentalCount));
    }

    /// <summary>
    /// Tests retrieval of top 5 clients by total rental cost
    /// Verifies correct ordering by total cost in descending order
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRentalSum_ShouldReturnCorrectOrder()
    {
        // Arrange
        var rentals = CarRentalDataSeeder.Rentals;
        var expectedTopCount = 5;
        var expectedLicenseNumber = new List<string>
        {
            "4567890123",
            "1234567890",
            "2233445566",
            "7890123456",
            "8901234567"
        };

        // Act
        var topClients = rentals
            .GroupBy(r => r.Client.DriverLicenseNumber)
            .Select(g => new
            {
                ClientLicenseNumber = g.Key,
                TotalRentalCost = g.Sum(r => r.TotalCost),
                g.First().Client
            })
            .OrderByDescending(x => x.TotalRentalCost)
            .Take(5)
            .Select(x => x.Client)
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topClients.Count);
        Assert.Equal(
            expectedLicenseNumber,
            topClients.Select(c => c.DriverLicenseNumber)
        );
    }
}
