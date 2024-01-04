using Microsoft.EntityFrameworkCore;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Persistence.DataSeeders;

public static class DataSeederTaskStatus
{
    public static ModelBuilder SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskStatus>().HasData(new Priority { Id = 1, Name = "Awaiting execution" });
        modelBuilder.Entity<TaskStatus>().HasData(new Priority { Id = 2, Name = "At work" });
        modelBuilder.Entity<TaskStatus>().HasData(new Priority { Id = 3, Name = "Awaiting verification" });
        modelBuilder.Entity<TaskStatus>().HasData(new Priority { Id = 4, Name = "Done" });

        return modelBuilder;
    }
}