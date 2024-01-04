using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Bases.Repositories;
using TaskNinjaHub.Application.Entities.TaskStatuses.Interfaces;
using TaskNinjaHub.Application.Interfaces;

namespace TaskNinjaHub.Application.Entities.TaskStatuses.Repositories;

public class TaskStatusRepository : BaseRepository<Domain.TaskStatus>, ITaskStatusRepository
{
    public TaskStatusRepository(ITaskNinjaHubDbContext? context) : base((DbContext)context!)
    {

    }
}