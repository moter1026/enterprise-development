using CarRentalPoint.Domain.Interfaces;
using CarRentalPoint.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CarRentalPoint.Infrastructure.Repositories;

/// <summary>
/// Implementation of a generic repository using Entity Framework Core.
/// Provides basic CRUD operations for a given entity type.
/// </summary>
/// <typeparam name="T">Type of the entity.</typeparam>
public class EfRepository<T>(AppDbContext context) : IRepository<T> where T : class
{
    private readonly DbSet<T> _dbSet = context.Set<T>();

    /// <summary>
    /// Returns a queryable collection of entities for further filtering or querying.
    /// </summary>
    /// <returns>IQueryable of the entity type.</returns>
    public IQueryable<T> Query()
    {
        return _dbSet.AsQueryable();
    }

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>List of all entities of type T.</returns>
    public async Task<List<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    /// <summary>
    /// Adds a new entity to the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing entity in the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes an existing entity from the database asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        await context.SaveChangesAsync();
    }
}
