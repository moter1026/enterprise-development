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
    IRepository<Rental> rentalRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets a list of clients who have rented cars of a specific car model.
    /// </summary>
    /// <param name="modelId">The ID of the car model.</param>
    /// <returns>List of clients who rented cars of the specified model, ordered by full name.</returns>
    [HttpGet("clients-by-car-model/{modelId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ClientGetDto>>> GetClientsByCarModel(int modelId)
    {
        var clients = await rentalRepo.Query()
            .Where(r => r.Car != null
                        && r.Car.Generation != null
                        && r.Car.Generation.CarModelId == modelId)
            .Select(r => r.Client)
            .Distinct()
            .ToListAsync();

        return Ok(mapper.Map<List<ClientGetDto>>(clients));
    }

    /// <summary>
    /// Gets a list of cars that are currently rented at the specified time.
    /// </summary>
    /// <param name="currentTime">The current time to check for active rentals.</param>
    /// <returns>List of cars currently rented, including generation and model details.</returns>
    [HttpGet("active-rentals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarGetDto>>> GetCarsCurrentlyRented([FromQuery] DateTime currentTime)
    {
        var activeRentals = await rentalRepo.Query()
            .Where(r => r.IsActive && r.Car != null)
            .Select(r => r.Car!)
            .Distinct()
            .Include(c => c.Generation)
                .ThenInclude(g => g!.Model)
            .ToListAsync();

        return Ok(mapper.Map<List<CarGetDto>>(activeRentals));
    }

    /// <summary>
    /// Gets the top 5 most frequently rented cars.
    /// </summary>
    /// <returns>List of the 5 cars with the highest rental count, including generation and model details.</returns>
    [HttpGet("top-5-cars")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarGetDto>>> GetTop5MostFrequentlyRentedCars()
    {
        var topCars = await rentalRepo.Query()
            .GroupBy(r => r.CarId)
            .Select(g => new
            {
                CarId = g.Key,
                Count = g.Count()
            })
            .OrderByDescending(x => x.Count)
            .Take(5)
            .Join(
                carRepo.Query()
                    .Include(c => c.Generation)
                        .ThenInclude(g => g!.Model),
                g => g.CarId,
                c => c.Id,
                (_, c) => c
            )
            .ToListAsync();

        return Ok(mapper.Map<List<CarGetDto>>(topCars));
    }

    /// <summary>
    /// Gets the total number of rentals per car.
    /// </summary>
    /// <returns>List of cars along with their total rental count.</returns>
    [HttpGet("rental-count-per-car")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<RentalCountPerCarDto>>> GetRentalCountPerCar()
    {
        var result = (await rentalRepo.Query()
            .Where(r => r.Car != null && r.Car.Generation != null)
            .GroupBy(r => r.Car!.Id)
            .Select(g => new
            {
                Car = g.Select(r => r.Car!).First(),
                RentalCount = g.Count()
            })
            .ToListAsync())
            .Select(x => new RentalCountPerCarDto(
                Id: x.Car.Id,
                LicensePlate: x.Car.LicensePlate,
                Color: x.Car.Color,
                Generation: mapper.Map<ModelGenerationGetDto>(x.Car.Generation),
                RentalCount: x.RentalCount
            ))
            .ToList();

        return Ok(result);
    }

    /// <summary>
    /// Gets the top 5 clients ranked by total rental cost.
    /// </summary>
    /// <returns>List of top clients with their total rental sum.</returns>
    [HttpGet("top-clients-by-rental-sum")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TopClientRentalSumDto>>> GetTopClientsByRentalSum()
    {
        var result = await rentalRepo.Query()
            .GroupBy(r => new { r.Client!.Id, r.Client.FullName, r.Client.DriverLicenseNumber, r.Client.BirthDate })
            .Select(g => new TopClientRentalSumDto(
                g.Key.Id,
                g.Key.FullName,
                g.Key.DriverLicenseNumber,
                g.Key.BirthDate,
                g.Sum(r => r.RentalHours * r.Car!.Generation!.RentalCostPerHour)
            ))
            .OrderByDescending(x => x.TotalRentalCost)
            .Take(5)
            .ToListAsync();

        return Ok(result);
    }
}
