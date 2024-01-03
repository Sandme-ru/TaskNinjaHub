using Microsoft.AspNetCore.Components.Authorization;

namespace TaskNinjaHub.WebClient.Services;

public interface IUserProviderService
{
    string UserName { get; set; }

    Task GetUser();
}

public class UserProviderService : IUserProviderService
{
    private readonly AuthenticationStateProvider _stateProvider;

    public string UserName { get; set; } = string.Empty;

    public UserProviderService(AuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }
    
    public async Task GetUser()
    {
        var authenticationState = await _stateProvider.GetAuthenticationStateAsync();
        var shortName = authenticationState.User?.FindFirst("short_name")?.Value;

        UserName = shortName ?? string.Empty;
    }
}