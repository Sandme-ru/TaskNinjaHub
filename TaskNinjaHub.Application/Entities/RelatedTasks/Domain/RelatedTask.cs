using TaskNinjaHub.Application.BaseUsers;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Interfaces.Haves;

namespace TaskNinjaHub.Application.Entities.RelatedTasks.Domain;

public class RelatedTask : BaseUserCU, IHaveId
{
    public int Id { get; set; }

    public int MainTaskId { get; set; }

    public virtual CatalogTask? MainTask { get; set; }

    public int SubordinateTaskId { get; set; }

    public virtual CatalogTask? SubordinateTask { get; set; }
}