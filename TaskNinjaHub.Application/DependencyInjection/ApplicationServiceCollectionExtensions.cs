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
using TaskNinjaHub.Application.Entities.RelatedTasks.Interfaces;
using TaskNinjaHub.Application.Entities.RelatedTasks.Repositories;
using TaskNinjaHub.Application.Entities.Tasks.Interfaces;
using TaskNinjaHub.Application.Entities.Tasks.Repositories;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.Application.Entities.TaskStatuses.Repositories;
using TaskNinjaHub.Application.Entities.TaskTypes.Interfaces;
using TaskNinjaHub.Application.Entities.TaskTypes.Repositories;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Application.Utilities;

namespace TaskNinjaHub.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        #region REPOSITORY

        services.AddScoped<IAuthorRepository, AuthorRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IInformationSystemRepository, InformationSystemRepository>();
        services.AddScoped<IPriorityRepository, PriorityRepository>();
        services.AddScoped<ITaskRepository, TaskRepository>();
        services.AddScoped<IRelatedTaskRepository, RelatedTaskRepository>();
        services.AddScoped<ITaskStatusRepository, TaskStatusRepository>();
        services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();

        #endregion

        #region SERVICE

        services.AddScoped<IEmailService, EmailService>();

        #endregion

        return services;
    }
}