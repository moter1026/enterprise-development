using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// API controller for managing cars.
/// </summary>
[ApiController]
[Route("api/cars")]
public class CarsController(
    IRepository<Car> carRepo,
    IRepository<ModelGeneration> genRepo,
    IRepository<Rental> rentalRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all cars, including their generation and model details.
    /// </summary>
    /// <returns>List of cars.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<CarGetDto>>> GetAll()
    {
        var cars = await carRepo.Query()
            .Include(c => c.Generation)
                .ThenInclude(g => g!.Model)
            .ToListAsync();

        return Ok(mapper.Map<List<CarGetDto>>(cars));
    }

    /// <summary>
    /// Gets a specific car by ID, including its generation and model details.
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <returns>The car if found; otherwise, 404.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CarGetDto>> GetById(int id)
    {
        var car = await carRepo.Query()
            .Include(c => c.Generation)
                .ThenInclude(g => g!.Model)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
            return NotFound();

        return Ok(mapper.Map<CarGetDto>(car));
    }

    /// <summary>
    /// Creates a new car.
    /// </summary>
    /// <param name="dto">The car data.</param>
    /// <returns>The created car.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<CarGetDto>> Create([FromBody] CarEditDto dto)
    {
        var generation = await genRepo.GetByIdAsync(dto.GenerationId);
        if (generation == null)
            return BadRequest($"ModelGeneration with Id={dto.GenerationId} not found.");

        var car = new Car
        {
            LicensePlate = dto.LicensePlate,
            Color = dto.Color,
            ModelGenerationId = generation.Id,
            Generation = generation
        };

        await carRepo.AddAsync(car);

        return CreatedAtAction(
            nameof(GetById),
            new { id = car.Id },
            mapper.Map<CarGetDto>(car)
        );
    }

    /// <summary>
    /// Updates an existing car.
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <param name="dto">Updated car data</param>
    /// <returns>NoContent if successful; 404 if not found; 400 if generation not found.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] CarEditDto dto)
    {
        var car = await carRepo.GetByIdAsync(id);
        if (car == null)
            return NoContent();

        var generation = await genRepo.GetByIdAsync(dto.GenerationId);
        if (generation == null)
            return BadRequest($"ModelGeneration with Id={dto.GenerationId} not found.");

        car.LicensePlate = dto.LicensePlate;
        car.Color = dto.Color;
        car.Generation = generation;

        await carRepo.UpdateAsync(car);
        return NoContent();
    }

    /// <summary>
    /// Deletes a car by ID.
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <returns>NoContent if deleted; 404 if not found; 400 if referenced in rentals.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await carRepo.Query()
            .Include(c => c.Generation)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (car == null)
            return NotFound();

        var hasRentals = await rentalRepo.Query().AnyAsync(r => r.Car!.Id == id);
        if (hasRentals)
            return BadRequest("Cannot delete car: it is referenced in existing rentals.");

        await carRepo.DeleteAsync(car);
        return NoContent();
    }
}
