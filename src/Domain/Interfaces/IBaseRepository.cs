using IdentityUser.src.Domain.Common;

namespace IdentityUser.src.Domain.Interfaces
{
    /// <summary>
    /// Interface for base repository operations.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        Task<List<T>> GetAll();

        /// <summary>
        /// Gets an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity.</param>
        /// <returns>The entity with the specified identifier.</returns>
        Task<T> GetById(Guid id);

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity.</returns>
        Task<T> Create(T entity);

        /// <summary>
        /// Updates an existing entity.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        Task<T> Update(T entity);

        /// <summary>
        /// Deletes an entity by its identifier.
        /// </summary>
        /// <param name="id">The identifier of the entity to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        Task<bool> Delete(Guid id);
    }
}
