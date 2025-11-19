using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Enums;
using CarRentalPoint.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// API controller for managing car models.
/// </summary>
[ApiController]
[Route("api/carmodels")]
public class CarModelsController(
    IRepository<CarModel> carModelRepo,
    IRepository<ModelGeneration> genRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all car models.
    /// </summary>
    /// <returns>List of all car models.</returns>
    [HttpGet]
    public async Task<ActionResult<List<CarModelGetDto>>> GetAll()
    {
        var models = await carModelRepo.Query().ToListAsync();
        return mapper.Map<List<CarModelGetDto>>(models);
    }

    /// <summary>
    /// Gets a car model by its ID.
    /// </summary>
    /// <param name="id">The ID of the car model.</param>
    /// <returns>The car model if found, otherwise 404.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<CarModelGetDto>> GetById(int id)
    {
        var model = await carModelRepo.GetByIdAsync(id);
        if (model == null)
            return NotFound();

        return mapper.Map<CarModelGetDto>(model);
    }

    /// <summary>
    /// Creates a new car model.
    /// </summary>
    /// <param name="dto">The data for creating the car model.</param>
    /// <returns>The created car model.</returns>
    [HttpPost]
    public async Task<ActionResult<CarModelGetDto>> Create([FromBody] CarModelEditDto dto)
    {
        if (!TryParseCarModelEnums(dto, out var driveType, out var bodyType, out var carClass, out var error))
            return BadRequest(error);

        var model = new CarModel
        {
            Name = dto.Name,
            DriveType = driveType,
            SeatsCount = dto.SeatsCount,
            BodyType = bodyType,
            CarClass = carClass
        };

        await carModelRepo.AddAsync(model);

        return CreatedAtAction(
            nameof(GetById),
            new { id = model.Id },
            mapper.Map<CarModelGetDto>(model)
        );
    }

    /// <summary>
    /// Updates an existing car model.
    /// </summary>
    /// <param name="id">The ID of the car model to update.</param>
    /// <param name="dto">The updated data for the car model.</param>
    /// <returns>NoContent if successful, 404 if not found, 400 if enums are invalid.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CarModelEditDto dto)
    {
        var model = await carModelRepo.GetByIdAsync(id);
        if (model == null)
            return NotFound();

        if (!TryParseCarModelEnums(dto, out var driveType, out var bodyType, out var carClass, out var error))
            return BadRequest(error);

        model.Name = dto.Name;
        model.DriveType = driveType;
        model.SeatsCount = dto.SeatsCount;
        model.BodyType = bodyType;
        model.CarClass = carClass;

        await carModelRepo.UpdateAsync(model);
        return NoContent();
    }

    /// <summary>
    /// Deletes a car model by ID.
    /// </summary>
    /// <param name="id">The ID of the car model to delete.</param>
    /// <returns>NoContent if deleted, 404 if not found, 400 if it has related generations.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var model = await carModelRepo.Query().FirstOrDefaultAsync(m => m.Id == id);
        if (model == null)
            return NotFound();

        var hasGenerations = await genRepo.Query().AnyAsync(g => g.Model.Id == id);
        if (hasGenerations)
            return BadRequest("Cannot delete CarModel: it has related ModelGenerations.");

        await carModelRepo.DeleteAsync(model);
        return NoContent();
    }

    /// <summary>
    /// Tries to parse string enum values from DTO into strongly-typed enums.
    /// </summary>
    private bool TryParseCarModelEnums(
        CarModelEditDto dto,
        out CarDriveType driveType,
        out CarBodyType bodyType,
        out CarClass carClass,
        out string error)
    {
        error = "";
        driveType = default;
        bodyType = default;
        carClass = default;

        if (!Enum.TryParse<CarDriveType>(dto.DriveType, true, out driveType))
        {
            error = $"Invalid DriveType: {dto.DriveType}. Allowed: {string.Join(", ", Enum.GetNames(typeof(CarDriveType)))}";
            return false;
        }

        if (!Enum.TryParse<CarBodyType>(dto.BodyType, true, out bodyType))
        {
            error = $"Invalid BodyType: {dto.BodyType}. Allowed: {string.Join(", ", Enum.GetNames(typeof(CarBodyType)))}";
            return false;
        }

        if (!Enum.TryParse<CarClass>(dto.CarClass, true, out carClass))
        {
            error = $"Invalid CarClass: {dto.CarClass}. Allowed: {string.Join(", ", Enum.GetNames(typeof(CarClass)))}";
            return false;
        }

        return true;
    }
}
