using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;

/// <summary>
/// Interface ITaskStatusRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus}" />
public interface ITaskStatusRepository : IBaseRepository<Domain.TaskStatus>
{

}