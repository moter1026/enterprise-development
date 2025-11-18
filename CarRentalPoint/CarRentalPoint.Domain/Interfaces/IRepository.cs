namespace CarRentalPoint.Domain.Interfaces;

/// <summary>
/// Generic repository interface providing common data access operations.
/// </summary>
/// <typeparam name="T">Type of the entity.</typeparam>
public interface IRepository<T> where T : class
{
    /// <summary>
    /// Creates a queryable collection of entities.
    /// </summary>
    /// <returns>IQueryable for further filtering and querying.</returns>
    public IQueryable<T> Query();

    /// <summary>
    /// Retrieves all entities asynchronously.
    /// </summary>
    /// <returns>List of all entities.</returns>
    public Task<List<T>> GetAllAsync();

    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the entity.</param>
    /// <returns>The entity if found; otherwise, null.</returns>
    public Task<T?> GetByIdAsync(int id);

    /// <summary>
    /// Adds a new entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    public Task AddAsync(T entity);

    /// <summary>
    /// Updates an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    public Task UpdateAsync(T entity);

    /// <summary>
    /// Deletes an existing entity asynchronously.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    public Task DeleteAsync(T entity);
}

