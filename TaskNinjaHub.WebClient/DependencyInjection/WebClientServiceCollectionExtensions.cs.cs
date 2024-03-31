using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient.DependencyInjection;

public static class WebClientServiceCollectionExtensions
{
    public static IServiceCollection AddWebClientServiceCollection(this IServiceCollection services)
    {
        services.AddScoped<AuthorService>();
        services.AddScoped<InformationSystemService>();
        services.AddScoped<PriorityService>();
        services.AddScoped<CatalogTaskService>();
        services.AddScoped<FileService>();
        services.AddScoped<TaskStatusService>();
        services.AddScoped<AuthService>();
        services.AddScoped<RelatedTaskService>();
        services.AddScoped<MinioService>();
        services.AddScoped<MachineLearningService>();

        return services;
    }
}