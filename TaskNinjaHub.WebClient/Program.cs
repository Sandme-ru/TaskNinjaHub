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
using TaskNinjaHub.WebClient.Services.Bases;

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

        #if (DEBUG)

        var authUrl = builder.Configuration.GetSection("HttpClientSettings:DebugAuthUrl").Value;
        var apiUrl = builder.Configuration.GetSection("HttpClientSettings:DebugApiUrl").Value;

        #elif (RELEASE)
        
        var authUrl = builder.Configuration.GetSection("HttpClientSettings:ReleaseAuthUrl").Value;
        var apiUrl = builder.Configuration.GetSection("HttpClientSettings:ReleaseApiUrl").Value;

        #endif

        builder.Services.AddRazorPages();
        builder.Services.AddServerSideBlazor();
        builder.Services.AddAntDesign();

        builder.Services.AddTransient(sp => new HttpClient
        {
            BaseAddress = new Uri(apiUrl!)
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

                options.Authority = authUrl;
                options.ClientId = "TaskNinjaHub";
                options.ClientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3655";
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.RequireHttpsMetadata = true;

                options.Scope.Clear();
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("offline_access");

                options.CallbackPath = "/task-ninja-hub/signin-oidc";

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

        app.Use((context, next) =>
        {
            context.Request.Scheme = "https";
            return next();
        });

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