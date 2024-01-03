using Microsoft.AspNetCore.Components.Authorization;
using TaskNinjaHub.Application.Entities.Authors.Domain;

namespace TaskNinjaHub.WebClient.Services;

public interface IUserProviderService
{
    Author User { get; set; }

    Task GetUser();
}

public class UserProviderService : IUserProviderService
{
    private readonly AuthenticationStateProvider _stateProvider;

    public Author User { get; set; } = null!;

    public UserProviderService(AuthenticationStateProvider stateProvider)
    {
        _stateProvider = stateProvider;
    }
    
    public async Task GetUser()
    {
        var authenticationState = await _stateProvider.GetAuthenticationStateAsync();
        var shortName = authenticationState.User?.FindFirst("short_name")?.Value;
        var roleName = authenticationState.User?.FindFirst("role_name")?.Value;

        User = new Author
        {
            Name = shortName ?? string.Empty,
            RoleName = roleName
        };
    }
}