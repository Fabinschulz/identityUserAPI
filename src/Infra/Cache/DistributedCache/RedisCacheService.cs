using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IdentityUser.src.Infra.Cache.DistributedCache
{
    /// <summary>
    /// Provides methods for interacting with a distributed cache.
    /// </summary>
    public class RedisCacheService : IDistributedCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCacheService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RedisCacheService"/> class.
        /// </summary>
        /// <param name="cache">The distributed cache instance to be used for caching operations.</param>
        /// <param name="logger">The logger instance to be used for logging operations.</param>
        /// <returns>
        /// A new instance of the <see cref="RedisCacheService"/> class.
        /// </returns>
        public RedisCacheService(IDistributedCache cache, ILogger<RedisCacheService> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Asynchronously retrieves a value from the cache for the specified key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="cacheKey">The key of the cache entry to retrieve.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains the value from the cache if found; otherwise, the default value for the type <typeparamref name="T"/>.
        /// </returns>
        /// <exception cref="Exception">Thrown when an error occurs while retrieving the value from the cache.</exception>

        public async Task<T?> GetValueAsync<T>(string cacheKey)
        {
            try
            {
                _logger.LogInformation("Buscando dados do cache para a chave: {CacheKey}", cacheKey);
                var cachedData = await _cache.GetStringAsync(cacheKey);

                if (!string.IsNullOrEmpty(cachedData))
                {
                    _logger.LogInformation("Dados encontrados no cache para a chave: {CacheKey}", cacheKey);

                    // Configurar a serialização para lidar com enums
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        Converters = { new JsonStringEnumConverter() }
                    };

                    return JsonSerializer.Deserialize<T>(cachedData!, options);
                }

                _logger.LogInformation("Dados não encontrados no cache para a chave: {CacheKey}", cacheKey);
                return default;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar dados do cache para a chave: {CacheKey}", cacheKey);
                throw;
            }
        }

        /// <summary>
        /// Sets a value in the cache asynchronously.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="cacheKey">The key of the cache entry.</param>
        /// <param name="data">The value to set in the cache.</param>
        /// <param name="options">The cache entry options.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while setting the value in the cache.</exception>

        public async Task SetValueAsync<T>(string cacheKey, T data, DistributedCacheEntryOptions options)
        {
            try
            {
                _logger.LogInformation("Armazenando dados no cache para a chave: {CacheKey}", cacheKey);

                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new JsonStringEnumConverter() }
                };

                var jsonData = JsonSerializer.Serialize(data, jsonOptions);
                await _cache.SetStringAsync(cacheKey, jsonData, options);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao armazenar dados no cache para a chave: {CacheKey}", cacheKey);
                throw;
            }
        }

        /// <summary>
        /// Invalidates the cache for the specified cache key.
        /// </summary>
        /// <param name="cacheKey">The key of the cache entry to invalidate.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the cache was successfully invalidated.
        /// </returns>
        public async Task<bool> InvalidateCacheAsync(string cacheKey)
        {
            var existsCache = await ExistsCacheAsync(cacheKey);

            if (!existsCache)
            {
                _logger.LogInformation("Não há dados no cache para a chave: {CacheKey}", cacheKey);
                return false;
            }

            _logger.LogInformation("Removendo dados do cache para a chave: {CacheKey}", cacheKey);
            await _cache.RemoveAsync(cacheKey);
            return true;

        }

        /// <summary>
        /// Checks if a cache entry exists for the given cache key.
        /// </summary>
        /// <param name="cacheKey">The key of the cache entry to check.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a boolean value indicating whether the cache entry exists.</returns>
        /// <exception cref="Exception">Thrown when an error occurs while checking the cache.</exception>
        public async Task<bool> ExistsCacheAsync(string cacheKey)
        {
            try
            {
                _logger.LogInformation("Verificando existência de dados no cache para a chave: {CacheKey}", cacheKey);
                var cachedData = await _cache.GetStringAsync(cacheKey);
                return cachedData != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao verificar existência de dados no cache para a chave: {CacheKey}", cacheKey);
                throw;
            }
        }

        /// <summary>
        /// Gets the cache options for the distributed cache.
        /// </summary>
        /// <returns>
        /// A <see cref="DistributedCacheEntryOptions"/> object that specifies the cache entry options,
        /// including an absolute expiration relative to now of 30 minutes.
        /// </returns>
        public DistributedCacheEntryOptions GetCacheOptions()
        {
            return new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
        }
    }
}