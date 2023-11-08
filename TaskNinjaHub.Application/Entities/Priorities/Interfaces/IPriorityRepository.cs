using TaskNinjaHub.Application.Entities.Bases.Interfaces;

namespace TaskNinjaHub.Application.Entities.Priorities.Interfaces;

/// <summary>
/// Interface IPriorityRepository
/// Extends the <see cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Priorities.Domain.Priority}" />
/// </summary>
/// <seealso cref="TaskNinjaHub.Application.Entities.Bases.Interfaces.IBaseRepository{TaskNinjaHub.Application.Entities.Priorities.Domain.Priority}" />
public interface IPriorityRepository : IBaseRepository<Domain.Priority>
{

}