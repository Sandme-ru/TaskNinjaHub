using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Application.Interfaces;

/// <summary>
/// Interface IApplicationDbContext
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// Gets or sets the authors.
    /// </summary>
    /// <value>The authors.</value>
    DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Gets or sets the information systems.
    /// </summary>
    /// <value>The information systems.</value>
    DbSet<InformationSystem> InformationSystems { get; set; }

    /// <summary>
    /// Gets or sets the priorities.
    /// </summary>
    /// <value>The priorities.</value>
    DbSet<Priority> Priorities { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Gets or sets the tasks.
    /// </summary>
    /// <value>The tasks.</value>
    DbSet<CatalogTask> CatalogTasks { get; set; }

    /// <summary>
    /// Gets or sets the task statuses.
    /// </summary>
    /// <value>The task statuses.</value>
    DbSet<TaskStatus> TaskStatuses { get; set; }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    /// <value>The users.</value>
    DbSet<User> Users { get; set; }
}