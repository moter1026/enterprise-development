using CarRentalPoint.Contract;
using CarRentalPoint.Entities;
using CarRentalPoint.InitialData;

namespace CarRentalPoint.Tests;

/// <summary>
/// Unit tests for CarRentalService class functionality
/// Tests various business logic scenarios including client queries, rental calculations, and data aggregation
/// </summary>
public class CarRentalServiceTests(CarRentalDataSeeder Service) : IClassFixture<CarRentalDataSeeder>
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
        var result = Service.Rentals
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
        var rentals = Service.Rentals;
        var currentTime = new DateTime(2024, 1, 15, 14, 0, 0); // Фиксированное время для теста
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
    private bool IsRentalActive(Rental rental, DateTime currentTime)
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
        var rentals = Service.Rentals;
        var expectedTopCount = 5;

        // Ожидаемое множество машин (например, заранее знаем из сидера, что чаще всего брали вот эти)
        var expectedCars = new List<Car>
        {
            Service.Cars[0],  // Id = 1 (5 аренд)
            Service.Cars[3],  // Id = 4 (3 аренды)
            Service.Cars[5],  // Id = 6 (3 аренды)
            Service.Cars[7],  // Id = 8 (2 аренды)
            Service.Cars[10]  // Id = 11 (2 аренды)
        };


        // Act
        var topCars = rentals
            .GroupBy(r => r.Car.Id)
            .Select(g => new
             {
                 CarId = g.Key,
                 RentalCount = g.Count(),
                 Car = g.First().Car  // Берем первый объект Car из группы
             })
            .OrderByDescending(x => x.RentalCount)
            .Take(expectedTopCount)
            .Select(x => x.Car)
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topCars.Count);         // проверяем размер
        Assert.Equal(expectedCars.Select(c => c.Id), topCars.Select(c => c.Id));                   // проверяем точное совпадение
    }


    /// <summary>
    /// Tests calculation of rental counts for each car in the fleet
    /// Verifies that aggregate counts match individual car rental records
    /// </summary>
    [Fact]
    public void GetRentalCountPerCar_ShouldReturnCountForEachCar()
    {
        // Arrange
        var rentals = Service.Rentals;
        var cars = Service.Cars;

        // Ожидаемое количество аренд для каждой машины (из анализа данных в сидере)
        var expectedRentalCounts = new Dictionary<int, int>
        {
            { 1, 5 },   // Car Id = 1 (А001АА) - 5 аренд
            { 2, 1 },   // Car Id = 2 (В002ВВ) - 1 аренда
            { 3, 1 },   // Car Id = 3 (С003СС) - 1 аренда
            { 4, 3 },   // Car Id = 4 (D004DD) - 3 аренды
            { 5, 1 },   // Car Id = 5 (Е005ЕЕ) - 1 аренда
            { 6, 3 },   // Car Id = 6 (F006FF) - 3 аренды
            { 7, 1 },   // Car Id = 7 (G007GG) - 1 аренда
            { 8, 2 },   // Car Id = 8 (H008HH) - 2 аренды
            { 9, 1 },   // Car Id = 9 (I009II) - 1 аренда
            { 10, 1 },  // Car Id = 10 (J010JJ) - 1 аренда
            { 11, 2 },  // Car Id = 11 (K011KK) - 2 аренды
            { 12, 1 }   // Car Id = 12 (L012LL) - 1 аренда
        };

        // Act
        var rentalCounts = rentals
            .GroupBy(r => r.Car.Id)
            .Select(g => new
            {
                CarId = g.Key,
                RentalCount = g.Count(),
                Car = g.First().Car
            })
            .ToList();

        var totalRentals = rentalCounts.Sum(x => x.RentalCount);

        // Assert
        Assert.Equal(rentals.Count, totalRentals); // Проверяем общее количество аренд

        // Проверяем количество аренд для каждой машины
        foreach (var expected in expectedRentalCounts)
        {
            var actualCount = rentalCounts.FirstOrDefault(x => x.CarId == expected.Key)?.RentalCount ?? 0;
            Assert.Equal(expected.Value, actualCount);
        }

        // Дополнительная проверка: убеждаемся, что все машины из ожидаемого списка присутствуют в результатах
        foreach (var carId in expectedRentalCounts.Keys)
        {
            Assert.Contains(rentalCounts, x => x.CarId == carId);
        }
    }

    /// <summary>
    /// Tests retrieval of top 5 clients by total rental cost
    /// Verifies correct ordering by total cost in descending order
    /// </summary>
    [Fact]
    public void GetTop5ClientsByRentalSum_ShouldReturnCorrectOrder()
    {
        // Arrange
        var rentals = Service.Rentals;
        var expectedTopCount = 5;
        // Ожидаемые клиенты в правильном порядке (на основе РЕАЛЬНОЙ суммы аренд)
        var expectedClients = new List<Client>
        {
            Service.Clients[3],  // Clients[3] 
            Service.Clients[0],  // Clients[0]
            Service.Clients[11], // Clients[11]
            Service.Clients[6],  // Clients[6]
            Service.Clients[7]   // Clients[7]
        };

        // Рассчитаем реальные суммы для каждого клиента
        var clientCosts = rentals
            .GroupBy(r => r.Client.DriverLicenseNumber)
            .Select(g => new
            {
                Client = g.First().Client,
                TotalRentalCost = g.Sum(r => r.TotalCost),
                RentalCount = g.Count()
            })
            .OrderByDescending(x => x.TotalRentalCost)
            .ToList();

        // Act
        var topClients = rentals
            .GroupBy(r => r.Client.DriverLicenseNumber)
            .Select(g => new
            {
                ClientLicenseNumber = g.Key,
                TotalRentalCost = g.Sum(r => r.TotalCost),
                Client = g.First().Client
            })
            .OrderByDescending(x => x.TotalRentalCost)
            .Take(expectedTopCount)
            .Select(x => x.Client)  // Берем только клиентов
            .ToList();

        // Assert
        Assert.Equal(expectedTopCount, topClients.Count);

        // Проверяем точное совпадение клиентов
        Assert.Equal(
            expectedClients.Select(c => c.DriverLicenseNumber),
            topClients.Select(c => c.DriverLicenseNumber)
        );
    }
}
