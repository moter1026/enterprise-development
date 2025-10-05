using Xunit;
using CarRentalPoint.InitialData;
using CarRentalPoint.Entities;
using System.Linq;

namespace CarRentalPoint.Tests;
public class CarRentalServiceTests
{
    private readonly CarRentalService _service;

    public CarRentalServiceTests()
    {
        _service = new CarRentalService();
    }

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

    [Fact]
    public void GetCarsCurrentlyRented_ShouldReturnActiveRentals()
    {
        // Arrange
        // Для теста изменим даты некоторых аренд, чтобы они были активными
        var rentals = _service.Rentals;

        // Сделаем несколько аренд активными (установим дату в будущем)
        var futureDate = DateTime.Now.AddHours(-1); // Начало аренды 1 час назад
        rentals[0].RentalStart = futureDate;
        rentals[0].RentalHours = 48; // Аренда активна еще 47 часов

        rentals[1].RentalStart = futureDate;
        rentals[1].RentalHours = 24; // Аренда активна еще 23 часа

        // Act
        var activeRentals = rentals
            .Where(r => r.IsActive)
            .Select(r => r.Car)
            .Distinct()
            .ToList();

        // Проверяем, что все возвращенные автомобили действительно в аренде
        foreach (var car in activeRentals)
        {
            var isRented = rentals
                .Any(r => r.Car == car && r.IsActive);
            Assert.True(isRented);
        }
    }

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

        // Проверяем порядок (по убыванию количества аренд)
        for (var i = 0; i < topCars.Count - 1; i++)
        {
            Assert.True(topCars[i].RentalCount >= topCars[i + 1].RentalCount);
        }
    }

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

        // Проверяем, что сумма всех аренд равна общему количеству аренд
        var totalRentals = rentalCounts.Sum(x => x.RentalCount);
        Assert.Equal(_service.Rentals.Count, totalRentals);

        // Проверяем, что у каждого автомобиля правильное количество аренд
        foreach (var car in _service.Cars)
        {
            var expectedCount = _service.Rentals.Count(r => r.Car == car);
            var actualCount = rentalCounts.FirstOrDefault(x => x.Car == car)?.RentalCount ?? 0;
            Assert.Equal(expectedCount, actualCount);
        }
    }

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

        // Проверяем порядок (по убыванию суммы аренд)
        for (var i = 0; i < topClients.Count - 1; i++)
        {
            Assert.True(topClients[i].TotalRentalCost >= topClients[i + 1].TotalRentalCost);
        }
    }

    [Fact]
    public void RentalProperties_ShouldCalculateCorrectly()
    {
        var rental = _service.Rentals[0];
        var expectedCost = rental.Car?.Generation?.RentalCostPerHour * rental.RentalHours ?? 0;

        Assert.Equal(expectedCost, rental.TotalCost);
        Assert.NotNull(rental.ToString());
    }

    [Fact]
    public void TestSpecificScenarios()
    {
        // Тест 1: Проверяем клиентов конкретной модели
        var bmwClients = _service.Rentals
            .Where(r => r.Car?.Generation?.Model?.Name == "BMW X5")
            .Select(r => r.Client?.FullName)
            .Where(name => name != null)  // Фильтруем null значения
            .Distinct()
            .OrderBy(name => name)
            .ToList();

        Assert.NotNull(bmwClients);

        // Тест 2: Проверяем топ автомобилей
        var topCars = _service.Rentals
            .Where(r => r.Car != null)  // Фильтруем null автомобили
            .GroupBy(r => r.Car)
            .Select(g => new { Car = g.Key!, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToList();

        Assert.True(topCars.All(x => x.Count > 0));

        // Тест 3: Проверяем суммы аренд по клиентам
        var clientSums = _service.Rentals
            .Where(r => r.Client != null)
            .GroupBy(r => r.Client)
            .Select(g => new
            {
                Client = g.Key!,  // ! - утверждаем, что ключ не null
                TotalCost = g.Sum(r => r.TotalCost)
            })
            .ToDictionary(x => x.Client, x => x.TotalCost);

        Assert.NotEmpty(clientSums);
    }
}
