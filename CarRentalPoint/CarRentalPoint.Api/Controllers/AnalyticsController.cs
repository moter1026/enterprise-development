using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// Provides analytics endpoints for car rentals, clients, and cars.
/// </summary>
[ApiController]
[Route("api/analytics")]
public class AnalyticsController(
    IRepository<Car> carRepo,
    IRepository<Client> clientRepo,
    IRepository<Rental> rentalRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets a list of clients who have rented cars of a specific car model.
    /// </summary>
    /// <param name="modelId">The ID of the car model.</param>
    /// <returns>List of clients who rented cars of the specified model, ordered by full name.</returns>
    [HttpGet("clients-by-car-model/{modelId}")]
    public async Task<ActionResult<List<ClientGetDto>>> GetClientsByCarModel(int modelId)
    {
        var clients = await rentalRepo.Query()
            .Where(r => r.Car.Generation.Model.Id == modelId)
            .Select(r => r.Client)
            .Distinct()
            .OrderBy(c => c.FullName)
            .ToListAsync();

        return mapper.Map<List<ClientGetDto>>(clients);
    }

    /// <summary>
    /// Gets a list of cars that are currently rented at the specified time.
    /// </summary>
    /// <param name="currentTime">The current time to check for active rentals.</param>
    /// <returns>List of cars currently rented, including generation and model details.</returns>
    [HttpGet("active-rentals")]
    public async Task<ActionResult<List<CarGetDto>>> GetCarsCurrentlyRented([FromQuery] DateTime currentTime)
    {
        var activeRentals = await rentalRepo.Query()
            .Where(r =>
                r.RentalStart <= currentTime &&
                r.RentalStart.AddHours(r.RentalHours) >= currentTime)
            .Select(r => r.Car)
            .Distinct()
            .Include(c => c.Generation)
                .ThenInclude(g => g.Model)
            .ToListAsync();

        return mapper.Map<List<CarGetDto>>(activeRentals);
    }

    /// <summary>
    /// Gets the top 5 most frequently rented cars.
    /// </summary>
    /// <returns>List of the 5 cars with the highest rental count, including generation and model details.</returns>
    [HttpGet("top-5-cars")]
    public async Task<ActionResult<List<CarGetDto>>> GetTop5MostFrequentlyRentedCars()
    {
        var query = await rentalRepo.Query()
            .GroupBy(r => r.Car.Id)
            .Select(g => new
            {
                CarId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .ToListAsync();

        var cars = await carRepo.Query()
            .Where(c => query.Select(q => q.CarId).Contains(c.Id))
            .Include(c => c.Generation)
            .ThenInclude(g => g.Model)
            .ToListAsync();

        var orderedCars = query
            .Join(cars, q => q.CarId, c => c.Id, (_, c) => c)
            .ToList();

        return mapper.Map<List<CarGetDto>>(orderedCars);
    }

    /// <summary>
    /// Gets the total number of rentals per car.
    /// </summary>
    /// <returns>List of cars along with their total rental count.</returns>
    [HttpGet("rental-count-per-car")]
    public async Task<ActionResult<List<RentalCountPerCarDto>>> GetRentalCountPerCar()
    {
        var data = await rentalRepo.Query()
            .GroupBy(r => r.Car.Id)
            .Select(g => new
            {
                Car = g.First().Car,
                RentalCount = g.Count()
            })
            .ToListAsync();

        var result = data.Select(x => new RentalCountPerCarDto(
            Id: x.Car.Id,
            LicensePlate: x.Car.LicensePlate,
            Color: x.Car.Color,
            Generation: mapper.Map<ModelGenerationGetDto>(x.Car.Generation),
            RentalCount: x.RentalCount
        )).ToList();

        return result;
    }

    /// <summary>
    /// Gets the top 5 clients ranked by total rental cost.
    /// </summary>
    /// <returns>List of top clients with their total rental sum.</returns>
    [HttpGet("top-clients-by-rental-sum")]
    public async Task<ActionResult<List<TopClientRentalSumDto>>> GetTopClientsByRentalSum()
    {
        var grouped = await rentalRepo.Query()
            .GroupBy(r => r.Client.Id)
            .Select(g => new
            {
                ClientId = g.Key,
                TotalCost = g.Sum(r =>
                    r.RentalHours * r.Car.Generation.RentalCostPerHour
                )
            })
            .OrderByDescending(x => x.TotalCost)
            .Take(5)
            .ToListAsync();

        var clientIds = grouped.Select(x => x.ClientId).ToList();

        var clients = await clientRepo.Query()
            .Where(c => clientIds.Contains(c.Id))
            .ToListAsync();

        var result = grouped
            .Join(clients,
                  g => g.ClientId,
                  c => c.Id,
                  (g, c) => new TopClientRentalSumDto(
                        c.Id,
                        c.FullName,
                        c.DriverLicenseNumber,
                        c.BirthDate,
                        g.TotalCost
                  ))
            .ToList();

        return result;
    }
}
