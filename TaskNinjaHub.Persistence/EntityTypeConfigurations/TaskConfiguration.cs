using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskNinjaHub.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.Persistence.EntityTypeConfigurations;

public class TaskConfiguration : IEntityTypeConfiguration<CatalogTask>
{
    public void Configure(EntityTypeBuilder<CatalogTask> builder)
    {
        builder.HasKey(note => note.Id);
        builder.HasIndex(note => note.Id).IsUnique();
        builder.Property(note => note.Name).HasMaxLength(500);

        builder
            .HasOne(x => x.TaskAuthor)
            .WithMany(x => x.AssignedTasks)
            .HasForeignKey(x => x.TaskAuthorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.TaskExecutor)
            .WithMany(x => x.ExecutableTasks)
            .HasForeignKey(x => x.TaskExecutorId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasMany(x => x.Files)
            .WithOne(x => x.Task)
            .HasForeignKey(x => x.TaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}