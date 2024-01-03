using Microsoft.AspNetCore.Authorization;
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

    private readonly AuthorService _authorService;

    public Author User { get; set; } = new();

    public UserProviderService(AuthenticationStateProvider stateProvider, AuthorService authorService)
    {
        _stateProvider = stateProvider;
        _authorService = authorService;
    }

    public async Task GetUser()
    {
        var authenticationState = await _stateProvider.GetAuthenticationStateAsync();

        var shortName = authenticationState.User.FindFirst("short_name")?.Value;
        var roleName = authenticationState.User?.FindFirst("role_name")?.Value;

        User = new Author
        {
            Name = shortName ?? string.Empty,
            RoleName = roleName
        };

        var user = ((await _authorService.GetAllByFilterAsync(User))!).FirstOrDefault()!;

        if (user != null)
        {
            User = user;
        }
    }
}