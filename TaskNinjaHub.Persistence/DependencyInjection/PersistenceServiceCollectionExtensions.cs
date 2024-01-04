using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Persistence.DependencyInjection;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        bool.TryParse(configuration["Logging:Console:Enabled"], out var consoleEnabled);

        #if (DEBUG)
        
        var connectionString = configuration["ConnectionStrings:TaskNinjaHub"];

        #elif (RELEASE)
        
        var connectionString = configuration["ConnectionStrings:ReleaseConnection"];

        #endif

        services.AddDbContext<TaskNinjaHubDbContext>(options =>
        {
            if (consoleEnabled)
            {
                options.UseLoggerFactory(LoggerFactory.Create(builder => { builder.AddConsole(); }));
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }

            options.UseLazyLoadingProxies();
            options.UseNpgsql(connectionString, o =>
            {
                o.CommandTimeout(600);
                o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                o.MigrationsHistoryTable("__ef_migrations_history");
            }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<ITaskNinjaHubDbContext>(provider => provider.GetRequiredService<TaskNinjaHubDbContext>());
        return services;
    }
}
