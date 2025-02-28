using StackExchange.Redis;

namespace IdentityUser.src.Infra.Cache.ElastiCacheRedis
{
    /// <summary>
    /// Provides extension methods for configuring ElastiCache Redis in an ASP.NET Core application.
    /// </summary>
    public static class ElastiCacheRedis
    {
        /// <summary>
        /// Adds Redis configuration to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add the Redis configuration to.</param>
        /// <param name="configuration">The configuration containing the Redis endpoint.</param>
        /// <exception cref="ArgumentException">Thrown when the Redis endpoint configuration is missing or empty.</exception>
        /// <exception cref="Exception">Thrown when there is an error connecting to Redis.</exception>
        public static void AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            var redisEndpoint = configuration["Redis"];

            if (string.IsNullOrEmpty(redisEndpoint))
            {
                throw new ArgumentException("Missing configuration values for ElastiCache Redis");
            }

            try
            {

                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisEndpoint;
                    options.InstanceName = "IDENTITY_";
                });

                ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisEndpoint);
                services.AddSingleton<IConnectionMultiplexer>(redis);
                Console.WriteLine("Redis IsConnected: " + redis.IsConnected);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao conectar ao Redis: " + ex.Message);
                throw;
            }
        }
    }
}