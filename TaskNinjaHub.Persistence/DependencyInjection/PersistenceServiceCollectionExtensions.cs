using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Persistence.DependencyInjection;

/// <summary>
/// Class PersistenceServiceCollectionExtensions.
/// </summary>
public static class PersistenceServiceCollectionExtensions
{
    /// <summary>
    /// Adds the persistence.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration["DbConnection"];
        
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(connectionString);
            options.EnableSensitiveDataLogging();
        });
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>()!);
        return services;
    }
}
