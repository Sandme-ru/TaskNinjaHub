using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Data.Authentication;

/// <summary>
/// Class WebsiteAuthenticator.
/// Implements the <see cref="AuthenticationStateProvider" />
/// </summary>
/// <seealso cref="AuthenticationStateProvider" />
public class WebsiteAuthenticator : AuthenticationStateProvider
{
    /// <summary>
    /// The protected local storage
    /// </summary>
    private readonly ProtectedLocalStorage _protectedLocalStorage;

    /// <summary>
    /// The user service
    /// </summary>
    private readonly UserService _userService;

    /// <summary>
    /// The user authentication service
    /// </summary>
    private readonly IUserAuthenticationService _userAuthenticationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="WebsiteAuthenticator" /> class.
    /// </summary>
    /// <param name="protectedLocalStorage">The protected local storage.</param>
    /// <param name="userService">The user service.</param>
    /// <param name="userAuthenticationService">The user authentication service.</param>
    public WebsiteAuthenticator(ProtectedLocalStorage protectedLocalStorage, 
        UserService userService, 
        IUserAuthenticationService userAuthenticationService)
    {
        _protectedLocalStorage = protectedLocalStorage;
        _userService = userService;
        _userAuthenticationService = userAuthenticationService;
    }

    /// <summary>
    /// Get authentication state as an asynchronous operation.
    /// </summary>
    /// <returns>A Task&lt;AuthenticationState&gt; representing the asynchronous operation.</returns>
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var principal = new ClaimsPrincipal();

        try
        {
            var storedPrincipal = await _protectedLocalStorage.GetAsync<string>("identity");

            if (storedPrincipal.Success)
            {
                var user = JsonConvert.DeserializeObject<User>(storedPrincipal.Value ?? string.Empty);
                _userAuthenticationService.AuthorizedUser = user;
                var (userInDb, isLookUpSuccess) = await LookUpUserAsync(user?.Username ?? string.Empty, user?.Password ?? string.Empty);

                if (isLookUpSuccess)
                {
                    var identity = CreateIdentityFromUserAsync(userInDb);
                    principal = new ClaimsPrincipal(await identity);
                }
            }
        }
        catch
        {
            // ignored
        }

        return new AuthenticationState(principal);
    }

    /// <summary>
    /// Login as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task LoginAsync(User user)
    {
        var (userInDatabase, isSuccess) = await LookUpUserAsync(user.Username, user.Password);
        _userAuthenticationService.AuthorizedUser = userInDatabase;

        var principal = new ClaimsPrincipal();

        if (isSuccess)
        {
            var identity = CreateIdentityFromUserAsync(userInDatabase);
            principal = new ClaimsPrincipal(await identity);
            var ret = JsonConvert.SerializeObject(userInDatabase);
            await _protectedLocalStorage.SetAsync("identity", ret);
        }

        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// Logout as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task LogoutAsync()
    {
        _userAuthenticationService.AuthorizedUser = null;

        await _protectedLocalStorage.DeleteAsync("identity");
        var principal = new ClaimsPrincipal();
        NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
    }

    /// <summary>
    /// Create identity from user as an asynchronous operation.
    /// </summary>
    /// <param name="user">The user.</param>
    /// <returns>A Task&lt;ClaimsIdentity&gt; representing the asynchronous operation.</returns>
    private async Task<ClaimsIdentity> CreateIdentityFromUserAsync(User user)
    {
        var result = new ClaimsIdentity(new Claim[]
        {
                new (ClaimTypes.Name, user.Username),
                new (ClaimTypes.Hash, user.Password),
                new (ClaimTypes.Role, user.Author?.Role?.Name ?? string.Empty),
        }, "WebsiteAuthenticator");

        return result;
    }

    /// <summary>
    /// Look up user as an asynchronous operation.
    /// </summary>
    /// <param name="username">The username.</param>
    /// <param name="password">The password.</param>
    /// <returns>A Task&lt;System.ValueTuple&gt; representing the asynchronous operation.</returns>
    private async Task<(User, bool)> LookUpUserAsync(string username, string password)
    {
        var users = await _userService?.GetAllAsync()!;

        if (users == null) 
            return (null, false)!;
        var result = users.FirstOrDefault(u => username == u.Username && password == u.Password);
        return (result, result is not null)!;
    }
}