using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskNinjaHub.Application.Entities.Authors.Interfaces;
using TaskNinjaHub.Application.Entities.Authors.Repositories;
using TaskNinjaHub.Application.Entities.Files.Interfaces;
using TaskNinjaHub.Application.Entities.Files.Repositories;
using TaskNinjaHub.Application.Entities.InformationSystems.Interfaces;
using TaskNinjaHub.Application.Entities.InformationSystems.Repositories;
using TaskNinjaHub.Application.Entities.Priorities.Interfaces;
using TaskNinjaHub.Application.Entities.Priorities.Repositories;
using TaskNinjaHub.Application.Entities.Roles.Interfaces;
using TaskNinjaHub.Application.Entities.Roles.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Repositories;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.Application.Entities.TaskStatuses.Repositories;
using TaskNinjaHub.Application.Entities.Users.Interfaces;
using TaskNinjaHub.Application.Entities.Users.Repositories;

namespace TaskNinjaHub.Application.DependencyInjection;

/// <summary>
/// Class ApplicationServiceCollectionExtensions.
/// </summary>
public static class ApplicationServiceCollectionExtensions
{
    /// <summary>
    /// Adds the persistence.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>IServiceCollection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        #region REPOSITORY

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IInformationSystemRepository, InformationSystemRepository>();
        services.AddScoped<IPriorityRepository, PriorityRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        #endregion

        return services;
    }
}