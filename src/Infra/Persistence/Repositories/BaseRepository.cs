using IdentityUser.src.Domain.Common;
using IdentityUser.src.Domain.Interfaces.Repositories;
using IdentityUser.src.Infra.Persistence.Database;
using Microsoft.EntityFrameworkCore;

namespace IdentityUser.src.Infra.Persistence.Repositories
{
    /// <summary>
    /// Base repository class providing common data access methods for entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// The database context used for database operations.
        /// </summary>
        protected readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRepository{T}"/> class.
        /// </summary>
        /// <param name="context">The database context used for database operations.</param>
        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all entities from the database.
        /// </summary>
        /// <returns>A list of all entities.</returns>
        public async Task<List<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        /// <summary>
        /// Retrieves an entity of type <typeparamref name="T"/> by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the entity of type <typeparamref name="T"/>.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when an entity with the specified ID is not found.</exception>
        public async Task<T> GetById(Guid id)
        {
            var user = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                throw new KeyNotFoundException($"Entidade com o ID '{id}' não foi encontrada.");
            }
            return user;
        }

        /// <summary>
        /// Creates a new entity in the database.
        /// </summary>
        /// <param name="entity">The entity to create.</param>
        /// <returns>The created entity.</returns>
        public async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return entity;
        }

        /// <summary>
        /// Updates an existing entity in the database.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <returns>The updated entity.</returns>
        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
            await Task.CompletedTask;
            return entity;
        }

        /// <summary>
        /// Deletes an entity by its ID.
        /// </summary>
        /// <param name="id">The ID of the entity to delete.</param>
        /// <returns>A boolean indicating whether the deletion was successful.</returns>
        public async Task<bool> Delete(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync(x => x.Id == id);

            if (entity != null)
            {
                _context.Set<T>().Update(entity);
                await _context.SaveChangesAsync();
                await Task.CompletedTask;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Saves all changes made in this context to the database.
        /// </summary>
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();

    }
}
