using IdentityUser.src.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// A hosted service that applies database migrations at startup.
/// </summary>
public class MigrationHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MigrationHostedService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="MigrationHostedService"/> class.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    /// <param name="logger">The logger.</param>
    public MigrationHostedService(IServiceProvider serviceProvider, ILogger<MigrationHostedService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Triggered when the application host is starting.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the start process is being canceled.</param>
    /// <returns>A Task that represents the completion of the start process.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {

        using (var scope = _serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            try
            {
                await context.Database.MigrateAsync(cancellationToken);
                _logger.LogInformation("Migração do banco de dados concluída com sucesso.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro durante a migração do banco de dados.");
                throw new Exception("Erro durante a migração do banco de dados.", ex);
            }

        }

    }

    /// <summary>
    /// Triggered when the application host is performing a graceful shutdown.
    /// </summary>
    /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
    /// <returns>A Task that represents the completion of the shutdown process.</returns>
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}