using Domain.Primitives;
using System.Linq.Expressions;

namespace Domain.Abstractions;

/// <summary>
/// Represents a repository for a specific type.
/// </summary>
/// <typeparam name="T">The type of entities the repository manages.</typeparam>
public interface IRepositoryBase<T> where T : BaseEntity
{
    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a read-only collection of entities.</returns>
    Task<IReadOnlyCollection<T>> GetAll();

    Task<bool> ExistsAsync(Expression<Func<T, bool>> filter);

    /// <summary>
    /// Gets entities from the repository based on a filter, order, and tracking options.
    /// </summary>
    /// <param name="filter">The filter to apply to the entities.</param>
    /// <param name="orderBy">The order to apply to the entities.</param>
    /// <param name="trackChanges">Whether to track changes on the entities.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains a read-only collection of entities.</returns>
    Task<IReadOnlyCollection<T>> Get(
        Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>,
        IOrderedQueryable<T>>? orderBy = null,
        bool trackChanges = false);

    /// <summary>
    /// Gets an entity by its ID.
    /// </summary>
    /// <param name="id">The ID of the entity.</param>
    /// <returns>A task that represents the asynchronous operation.
    /// The task result contains the entity, or null if not found.</returns>
    Task<T?> GetById(Guid id);

    /// <summary>
    /// Adds an entity to the repository.
    /// </summary>
    /// <param name="entity">The entity to add.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Add(T entity);

    /// <summary>
    /// Updates an entity in the repository.
    /// </summary>
    /// <param name="entity">The entity to update.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Update(T entity);

    /// <summary>
    /// Deletes an entity from the repository.
    /// </summary>
    /// <param name="entity">The entity to delete.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task Delete(T entity);

    Task Delete(Guid id);
}
