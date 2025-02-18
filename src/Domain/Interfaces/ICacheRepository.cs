using Microsoft.Extensions.Caching.Distributed;

namespace IdentityUser.src.Domain.Interfaces
{
    /// <summary>
    /// Interface for cache repository operations.
    /// </summary>
    public interface ICacheRepository
    {
        /// <summary>
        /// Retrieves a value from the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="cacheKey">The key of the cache entry.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the value from the cache.</returns>
        Task<T?> GetValueAsync<T>(string cacheKey);

        /// <summary>
        /// Sets a value in the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="cacheKey">The key of the cache entry.</param>
        /// <param name="data">The value to set in the cache.</param>
        /// <param name="options">The cache entry options.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SetValueAsync<T>(string cacheKey, T data, DistributedCacheEntryOptions options);

        /// <summary>
        /// Invalidates a cache entry asynchronously.
        /// </summary>
        /// <param name="cacheKey">The key of the cache entry to invalidate.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the cache entry was successfully invalidated.</returns>
        Task<bool> InvalidateCacheAsync(string cacheKey);

        /// <summary>
        /// Checks if a cache entry exists asynchronously.
        /// </summary>
        /// <param name="cacheKey">The key of the cache entry to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean indicating whether the cache entry exists.</returns>
        Task<bool> ExistsCacheAsync(string cacheKey);

        /// <summary>
        /// Gets the cache entry options.
        /// </summary>
        /// <returns>The cache entry options.</returns>
        DistributedCacheEntryOptions GetCacheOptions();
    }
}