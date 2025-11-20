using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// API controller for managing model generations of car models.
/// </summary>
[ApiController]
[Route("api/model-generations")]
public class ModelGenerationsController(
    IRepository<ModelGeneration> repo,
    IRepository<CarModel> modelRepo,
    IRepository<Car> carRepo,
    IMapper mapper) : ControllerBase
{
    private static readonly string[] _allowedTransmissions = ["Automatic", "Manual"];

    /// <summary>
    /// Gets all model generations.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ModelGenerationGetDto>>> GetAll()
    {
        var generations = await repo.Query()
            .Include(g => g.Model)
            .ToListAsync();

        return Ok(mapper.Map<List<ModelGenerationGetDto>>(generations));
    }

    /// <summary>
    /// Gets a model generation by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ModelGenerationGetDto>> GetById(int id)
    {
        var generation = await repo.Query()
            .Include(g => g.Model)
            .FirstOrDefaultAsync(g => g.Id == id);

        if (generation == null)
            return NotFound();

        return Ok(mapper.Map<ModelGenerationGetDto>(generation));
    }

    /// <summary>
    /// Creates a new model generation.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ModelGenerationGetDto>> Create([FromBody] ModelGenerationEditDto dto)
    {
        if (!_allowedTransmissions.Contains(dto.TransmissionType, StringComparer.OrdinalIgnoreCase))
            return BadRequest($"Invalid TransmissionType. Allowed values: {string.Join(", ", _allowedTransmissions)}");

        var model = await modelRepo.GetByIdAsync(dto.ModelId);
        if (model == null)
            return BadRequest($"CarModel with Id {dto.ModelId} not found.");

        var generation = new ModelGeneration
        {
            Year = dto.Year,
            EngineVolume = dto.EngineVolume,
            TransmissionType = dto.TransmissionType,
            CarModelId = model.Id,
            RentalCostPerHour = dto.RentalCostPerHour,
            Model = model
        };

        await repo.AddAsync(generation);
        return CreatedAtAction(nameof(GetById), new { id = generation.Id }, mapper.Map<ModelGenerationGetDto>(generation));
    }

    /// <summary>
    /// Updates an existing model generation.
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] ModelGenerationEditDto dto)
    {
        var generation = await repo.GetByIdAsync(id);
        if (generation == null)
            return NotFound();

        if (!_allowedTransmissions.Contains(dto.TransmissionType, StringComparer.OrdinalIgnoreCase))
            return BadRequest($"Invalid TransmissionType. Allowed values: {string.Join(", ", _allowedTransmissions)}");

        var model = await modelRepo.GetByIdAsync(dto.ModelId);
        if (model == null)
            return BadRequest($"CarModel with Id {dto.ModelId} not found.");

        generation.Year = dto.Year;
        generation.EngineVolume = dto.EngineVolume;
        generation.TransmissionType = dto.TransmissionType;
        generation.RentalCostPerHour = dto.RentalCostPerHour;
        generation.Model = model;

        await repo.UpdateAsync(generation);
        return NoContent();
    }

    /// <summary>
    /// Deletes a model generation by ID.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id)
    {
        var generation = await repo.Query().FirstOrDefaultAsync(g => g.Id == id);
        if (generation == null)
            return NoContent();

        var hasCars = await carRepo.Query().AnyAsync(c => c.Generation!.Id == id);
        if (hasCars)
            return BadRequest("Cannot delete ModelGeneration: it has related Cars.");

        await repo.DeleteAsync(generation);
        return NoContent();
    }
}
