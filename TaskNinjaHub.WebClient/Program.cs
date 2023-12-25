using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using TaskNinjaHub.WebClient.Data.Authentication;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient;

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

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddAntDesign();

        builder.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7179")
        });

        builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
        builder.Services.AddScoped<AuthorService>();
        builder.Services.AddScoped<InformationSystemService>();
        builder.Services.AddScoped<PriorityService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<RoleService>();
        builder.Services.AddScoped<CatalogTaskService>();
        builder.Services.AddScoped<FileService>();
        builder.Services.AddScoped<TaskStatusService>();
        builder.Services.AddSingleton<IUserAuthenticationService, UserAuthenticationService>();

        builder.Services.AddScoped<WebsiteAuthenticator>();
        builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<WebsiteAuthenticator>());

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}