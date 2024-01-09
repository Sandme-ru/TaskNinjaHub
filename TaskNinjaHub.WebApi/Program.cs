using TaskNinjaHub.Application.DependencyInjection;
using TaskNinjaHub.Persistence;
using TaskNinjaHub.Persistence.DependencyInjection;
using TaskNinjaHub.WebApi.Subdomain;

namespace TaskNinjaHub.WebApi;

/// <summary>
/// Class Program.
/// </summary>
public class Program
{
    /// <summary>
    /// Defines the entry point of the application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    public static void Main(string[] args)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddApplication(builder.Configuration);
        builder.Services.AddPersistence(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.DocumentFilter<SubdomainRouteAttribute>();
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                policyBuilder => policyBuilder.WithOrigins("https://localhost:7063")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
            var dbContext = serviceScope.ServiceProvider.GetRequiredService<TaskNinjaHubDbContext>();
            dbContext.MigrateDatabase();
        }

        app.UseCors("AllowSpecificOrigin");

        app.UseRouting();
        app.UseStaticFiles();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}