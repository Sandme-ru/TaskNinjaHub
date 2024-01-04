using Duende.AccessTokenManagement.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using OpenIddict.Server.AspNetCore;
using System.IdentityModel.Tokens.Jwt;
using TaskNinjaHub.WebClient.Data;
using TaskNinjaHub.WebClient.DependencyInjection;
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

        builder.Services.AddWebClientServiceCollection(builder.Configuration);

        #region AUTHENTICATION

        builder.Services.AddTransient<CookieEvents>();
        builder.Services.AddTransient<OidcEvents>();

        builder.Services.AddSingleton<IUserTokenStore, ServerSideTokenStore>();
        builder.Services.AddScoped<IUserProviderService, UserProviderService>();

        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
                options.DefaultSignOutScheme = OpenIddictServerAspNetCoreDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "Edison";
                options.EventsType = typeof(CookieEvents);
                options.Cookie.Path = "/";
            })
            .AddOpenIdConnect(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.AuthenticationMethod = OpenIdConnectRedirectBehavior.RedirectGet;
                options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                options.ClaimActions.MapUniqueJsonKey("office", "office");
                options.UsePkce = true;

                options.Authority = "https://localhost:7219";
                options.ClientId = "TaskNinjaHub";
                options.ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3655";
                options.ResponseType = OpenIdConnectResponseType.Code;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");

                options.TokenValidationParameters.NameClaimType = "name";

                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                options.EventsType = typeof(OidcEvents);
            });

        builder.Services.AddAuthorization();

        builder.Services.AddOpenIdConnectAccessTokenManagement();

        #endregion

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapBlazorHub();
        app.MapFallbackToPage("/_Host");

        app.Run();
    }
}