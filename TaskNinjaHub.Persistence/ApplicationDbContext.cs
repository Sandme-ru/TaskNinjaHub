using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Roles.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.Application.Interfaces;
using TaskNinjaHub.Persistence.DataSeeders;
using TaskNinjaHub.Persistence.EntityTypeConfigurations;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    /// <summary>
    /// Override this method to further configure the model that was discovered by convention from the entity types
    /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
    /// and re-used for subsequent instances of your derived context.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
    /// define extension methods on this object that allow you to configure aspects of the model that are specific
    /// to a given database.</param>
    /// <remarks><para>
    /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
    /// then this method will not be run. However, it will still run when creating a compiled model.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-modeling">Modeling entity types and relationships</see> for more information and
    /// examples.
    /// </para></remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TaskConfiguration());

        modelBuilder = DataSeederRole.SeedData(modelBuilder);
        modelBuilder = DataSeederAuthor.SeedData(modelBuilder);
        modelBuilder = DataSeederInformationSystem.SeedData(modelBuilder);
        modelBuilder = DataSeederPriority.SeedData(modelBuilder);
        modelBuilder = DataSeederTaskStatus.SeedData(modelBuilder);
        modelBuilder = DataSeederUser.SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    /// <summary>
    /// Override this method to configure the database (and other options) to be used for this context.
    /// This method is called for each instance of the context that is created.
    /// The base implementation does nothing.
    /// </summary>
    /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
    /// typically define extension methods on this object that allow you to configure the context.</param>
    /// <remarks><para>
    /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
    /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
    /// the options have already been set, and skip some or all of the logic in
    /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
    /// </para>
    /// <para>
    /// See <see href="https://aka.ms/efcore-docs-dbcontext">DbContext lifetime, configuration, and initialization</see>
    /// for more information and examples.
    /// </para></remarks>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseLazyLoadingProxies();
        optionsBuilder
            .ConfigureWarnings(x => x.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
    }

    #region Entities

    /// <summary>
    /// Gets or sets the tasks.
    /// </summary>
    /// <value>The tasks.</value>
    public DbSet<CatalogTask> CatalogTasks { get; set; }

    /// <summary>
    /// Gets or sets the authors.
    /// </summary>
    /// <value>The authors.</value>
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Gets or sets the information systems.
    /// </summary>
    /// <value>The information systems.</value>
    public DbSet<InformationSystem> InformationSystems { get; set; }

    /// <summary>
    /// Gets or sets the priorities.
    /// </summary>
    /// <value>The priorities.</value>
    public DbSet<Priority> Priorities { get; set; }

    /// <summary>
    /// Gets or sets the roles.
    /// </summary>
    /// <value>The roles.</value>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Gets or sets the task statuses.
    /// </summary>
    /// <value>The task statuses.</value>
    public DbSet<TaskStatus> TaskStatuses { get; set; }

    /// <summary>
    /// Gets or sets the users.
    /// </summary>
    /// <value>The users.</value>
    public DbSet<User> Users { get; set; }
    
    /// <summary>
    /// Gets or sets the files.
    /// </summary>
    /// <value>The users.</value>
    public DbSet<File> Files { get; set; }

    #endregion
}