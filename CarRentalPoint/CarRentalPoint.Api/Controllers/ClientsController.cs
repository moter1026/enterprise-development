using AutoMapper;
using CarRentalPoint.Application.Dtos;
using CarRentalPoint.Domain.Contract;
using CarRentalPoint.Domain.Entities;
using CarRentalPoint.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Api.Controllers;

/// <summary>
/// API controller for managing clients.
/// </summary>
[ApiController]
[Route("api/clients")]
public class ClientsController(
    IRepository<Client> repo,
    IRepository<Rental> rentalRepo,
    IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all clients.
    /// </summary>
    /// <returns>List of clients.</returns>
    [HttpGet]
    public async Task<ActionResult<List<ClientGetDto>>> GetAll()
    {
        var clients = await repo.Query().ToListAsync();
        return mapper.Map<List<ClientGetDto>>(clients);
    }

    /// <summary>
    /// Gets a client by ID.
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <returns>Client if found; 404 otherwise.</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ClientGetDto>> GetById(int id)
    {
        var client = await repo.GetByIdAsync(id);
        if (client == null)
            return NotFound();

        return mapper.Map<ClientGetDto>(client);
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="dto">Client data.</param>
    /// <returns>The created client.</returns>
    [HttpPost]
    public async Task<ActionResult<ClientGetDto>> Create([FromBody] ClientEditDto dto)
    {
        var client = new Client
        {
            FullName = dto.FullName,
            DriverLicenseNumber = dto.DriverLicenseNumber,
            BirthDate = dto.BirthDate
        };

        await repo.AddAsync(client);
        return CreatedAtAction(
            nameof(GetById),
            new { id = client.Id },
            mapper.Map<ClientGetDto>(client)
        );
    }

    /// <summary>
    /// Updates an existing client.
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <param name="dto">Updated client data</param>
    /// <returns>NoContent if successful; 404 if not found.</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientEditDto dto)
    {
        var client = await repo.GetByIdAsync(id);
        if (client == null)
            return NotFound();

        client.FullName = dto.FullName;
        client.DriverLicenseNumber = dto.DriverLicenseNumber;
        client.BirthDate = dto.BirthDate;

        await repo.UpdateAsync(client);
        return NoContent();
    }

    /// <summary>
    /// Deletes a client by ID.
    /// </summary>
    /// <param name="id">Client ID</param>
    /// <returns>NoContent if deleted; 404 if not found; 400 if client has rentals.</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var client = await repo.Query().FirstOrDefaultAsync(c => c.Id == id);
        if (client == null)
            return NotFound();

        var hasRentals = await rentalRepo.Query().AnyAsync(r => r.Client.Id == id);
        if (hasRentals)
            return BadRequest("Cannot delete client: it is referenced in existing rentals.");

        await repo.DeleteAsync(client);
        return NoContent();
    }
}
