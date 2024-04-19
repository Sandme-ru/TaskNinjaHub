using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services.HttpClientServices;
using TaskNinjaHub.WebClient.Services.Options;
using TaskNinjaHub.WebClient.Services.UserProviders;

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
        services.AddScoped<TaskTypeService>();

        services.AddScoped<IUserProviderService, UserProviderService>();
        services.AddScoped<IMachineLearningModeOptionService, MachineLearningModeOptionService>();

        return services;
    }
}