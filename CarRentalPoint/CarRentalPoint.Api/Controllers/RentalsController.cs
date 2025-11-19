using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Interfaces;
using CarRentalPoint.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// API controller for managing rentals.
/// </summary>
[ApiController]
[Route("api/rentals")]
public class RentalsController(
    IRepository<Rental> repo,
    IRepository<Client> clientRepo,
    IRepository<Car> carRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all rentals.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<RentalGetDto>>> GetAll()
    {
        var rentals = await repo.Query()
            .Include(r => r.Client)
            .Include(r => r.Car)
                .ThenInclude(c => c.Generation)
                    .ThenInclude(g => g.Model)
            .ToListAsync();

        return mapper.Map<List<RentalGetDto>>(rentals);
    }

    /// <summary>
    /// Gets a rental by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RentalGetDto>> GetById(int id)
    {
        var rental = await repo.Query()
            .Include(r => r.Client)
            .Include(r => r.Car)
                .ThenInclude(c => c.Generation)
                    .ThenInclude(g => g.Model)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (rental == null)
            return NotFound();

        return mapper.Map<RentalGetDto>(rental);
    }

    /// <summary>
    /// Creates a new rental.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RentalGetDto>> Create([FromBody] RentalEditDto dto)
    {
        var client = await clientRepo.GetByIdAsync(dto.ClientId);
        if (client == null)
            return BadRequest($"Client with Id {dto.ClientId} not found.");

        var car = await carRepo.Query()
            .Include(c => c.Generation)
                .ThenInclude(g => g.Model)
            .FirstOrDefaultAsync(c => c.Id == dto.CarId);

        if (car == null)
            return BadRequest($"Car with Id {dto.CarId} not found.");

        var rentalEnd = dto.RentalStart.AddHours(dto.RentalHours);
        var overlappingRental = await repo.Query()
            .Where(r => r.Car.Id == dto.CarId &&
                        r.RentalStart < rentalEnd &&
                        r.RentalStart.AddHours(r.RentalHours) > dto.RentalStart)
            .AnyAsync();
        if (overlappingRental)
            return BadRequest("Car is already rented during the selected period.");

        var rental = new Rental
        {
            Client = client,
            Car = car,
            RentalStart = dto.RentalStart,
            RentalHours = dto.RentalHours
        };

        await repo.AddAsync(rental);
        return CreatedAtAction(nameof(GetById), new { id = rental.Id }, mapper.Map<RentalGetDto>(rental));
    }

    /// <summary>
    /// Updates an existing rental.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] RentalEditDto dto)
    {
        var rental = await repo.GetByIdAsync(id);
        if (rental == null)
            return NotFound();

        var client = await clientRepo.GetByIdAsync(dto.ClientId);
        if (client == null)
            return BadRequest($"Client with Id {dto.ClientId} not found.");

        var car = await carRepo.GetByIdAsync(dto.CarId);
        if (car == null)
            return BadRequest($"Car with Id {dto.CarId} not found.");

        var rentalEnd = dto.RentalStart.AddHours(dto.RentalHours);
        var overlappingRental = await repo.Query()
            .Where(r => r.Car.Id == dto.CarId &&
                        r.Id != id &&
                        r.RentalStart < rentalEnd &&
                        r.RentalStart.AddHours(r.RentalHours) > dto.RentalStart)
            .AnyAsync();
        if (overlappingRental)
            return BadRequest("Car is already rented during the selected period.");

        rental.Client = client;
        rental.Car = car;
        rental.RentalStart = dto.RentalStart;
        rental.RentalHours = dto.RentalHours;

        await repo.UpdateAsync(rental);
        return NoContent();
    }

    /// <summary>
    /// Deletes a rental by ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var rental = await repo.GetByIdAsync(id);
        if (rental == null)
            return NotFound();

        await repo.DeleteAsync(rental);
        return NoContent();
    }
}
