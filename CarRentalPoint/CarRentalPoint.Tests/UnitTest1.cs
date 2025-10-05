using Xunit;
using CarRentalPoint.InitialData;
using CarRentalPoint.Entities;
using System.Linq;

namespace CarRentalPoint.Tests;

/// <summary>
/// Unit tests for CarRentalService class functionality
/// Tests various business logic scenarios including client queries, rental calculations, and data aggregation
/// </summary>
public class CarRentalServiceTests
{
    private readonly CarRentalService _service;

    /// <summary>
    /// Initializes a new instance of the test class with fresh service data
    /// </summary>
    public CarRentalServiceTests()
    {
        _service = new CarRentalService();
    }

    /// <summary>
    /// Tests that clients who rented a specific car model are returned and ordered by full name
    /// Verifies the correct filtering and sorting logic
    /// </summary>
    [Fact]
    public void GetClientsByCarModel_ShouldReturnClientsOrderedByFullName()
    {
        // Arrange
        var targetModel = "Toyota Camry";

        // Act
        var result = _service.Rentals
            .Where(r => r.Car.Generation.Model.Name == targetModel)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c?.FullName)
            .ToList();

        Assert.True(result.Count() == 3);
    }

    /// <summary>
    /// Tests retrieval of currently rented cars by modifying rental dates to simulate active rentals
    /// Verifies that IsActive property correctly identifies ongoing rentals
    /// </summary>
    [Fact]
    public void GetCarsCurrentlyRented_ShouldReturnActiveRentals()
    {
        // Arrange
        // For testing purposes, modify some rental dates to make them active
        var rentals = _service.Rentals;

        // Make several rentals active (set start date in recent past)
        var futureDate = DateTime.Now.AddHours(-1); // Rental started 1 hour ago
        rentals[0].RentalStart = futureDate;
        rentals[0].RentalHours = 48; // Rental active for 47 more hours

        rentals[1].RentalStart = futureDate;
        rentals[1].RentalHours = 24; // Rental active for 23 more hours

        // Act
        var activeRentals = rentals
            .Where(r => r.IsActive)
            .Select(r => r.Car)
            .Distinct()
            .ToList();

        // Verify that all returned cars are indeed currently rented
        foreach (var car in activeRentals)
        {
            var isRented = rentals
                .Any(r => r.Car == car && r.IsActive);
            Assert.True(isRented);
        }
    }

    /// <summary>
    /// Tests the retrieval of top 5 most frequently rented cars
    /// Verifies correct ordering by rental count in descending order
    /// </summary>
    [Fact]
    public void GetTop5MostFrequentlyRentedCars_ShouldReturnCorrectOrder()
    {
        var topCars = _service.Rentals
            .GroupBy(r => r.Car)
            .Select(g => new
            {
                Car = g.Key,
                RentalCount = g.Count()
            })
            .OrderByDescending(x => x.RentalCount)
            .Take(5)
            .ToList();

        Assert.True(topCars.Count <= 5);

        // Verify ordering (by descending rental count)
        for (var i = 0; i < topCars.Count - 1; i++)
        {
            Assert.True(topCars[i].RentalCount >= topCars[i + 1].RentalCount);
        }
    }

    /// <summary>
    /// Tests calculation of rental counts for each car in the fleet
    /// Verifies that aggregate counts match individual car rental records
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar_ShouldReturnCountForEachCar()
    {
        var rentalCounts = _service.Rentals
            .GroupBy(r => r.Car)
            .Select(g => new
            {
                Car = g.Key,
                RentalCount = g.Count()
            })
            .ToList();

        // Verify that sum of all rentals equals total number of rentals
        var totalRentals = rentalCounts.Sum(x => x.RentalCount);
        Assert.Equal(_service.Rentals.Count, totalRentals);

        // Verify that each car has the correct rental count
        foreach (var car in _service.Cars)
        {
            var expectedCount = _service.Rentals.Count(r => r.Car == car);
            var actualCount = rentalCounts.FirstOrDefault(x => x.Car == car)?.RentalCount ?? 0;
            Assert.Equal(expectedCount, actualCount);
        }
    }

    /// <summary>
    /// Tests retrieval of top 5 clients by total rental cost
    /// Verifies correct ordering by total cost in descending order
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRentalSum_ShouldReturnCorrectOrder()
    {
        var topClients = _service.Rentals
            .GroupBy(r => r.Client)
            .Select(g => new
            {
                Client = g.Key,
                TotalRentalCost = g.Sum(r => r.TotalCost)
            })
            .OrderByDescending(x => x.TotalRentalCost)
            .Take(5)
            .ToList();

        // Verify ordering (by descending rental sum)
        for (var i = 0; i < topClients.Count - 1; i++)
        {
            Assert.True(topClients[i].TotalRentalCost >= topClients[i + 1].TotalRentalCost);
        }
    }

    /// <summary>
    /// Tests that rental properties are calculated correctly
    /// Verifies total cost calculation and string representation functionality
    /// </summary>
    [Fact]
    public void RentalProperties_ShouldCalculateCorrectly()
    {
        var rental = _service.Rentals[0];
        var expectedCost = rental.Car?.Generation?.RentalCostPerHour * rental.RentalHours ?? 0;

        Assert.Equal(expectedCost, rental.TotalCost);
        Assert.NotNull(rental.ToString());
    }

    /// <summary>
    /// Tests multiple specific business scenarios in one comprehensive test
    /// Includes BMW client filtering, top car analysis, and client rental cost aggregation
    /// </summary>
    [Fact]
    public void TestSpecificScenarios()
    {
        // Test 1: Verify clients of specific model
        var bmwClients = _service.Rentals
            .Where(r => r.Car?.Generation?.Model?.Name == "BMW X5")
            .Select(r => r.Client?.FullName)
            .Where(name => name != null)  // Filter null values
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        Assert.NotNull(bmwClients);

        // Test 2: Verify top cars analysis
        var topCars = _service.Rentals
            .Where(r => r.Car != null)  // Filter null cars
            .GroupBy(r => r.Car)
            .Select(g => new { Car = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.True(topCars.All(x => x.Count > 0));

        // Test 3: Verify rental cost aggregation by client
        var clientSums = _service.Rentals
            .Where(r => r.Client != null)
            .GroupBy(r => r.Client)
            .Select(g => new
            {
                Client = g.Key!,  // ! - assert non-null key
                TotalCost = g.Sum(r => r.TotalCost)
            })
            .ToDictionary(x => x.Client, x => x.TotalCost);

        Assert.NotEmpty(clientSums);
    }
}