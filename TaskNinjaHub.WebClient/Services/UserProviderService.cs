using Microsoft.AspNetCore.Components.Authorization;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Services;

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
        var emailValue = authenticationState.User?.FindFirst("email_value")?.Value;

        User = new Author
        {
            Name = emailValue ?? string.Empty,
            RoleName = roleName,
            ShortName = shortName
        };

        Author? user = null;

        try
        {
            user = (await _authorService.GetAllByFilterAsync(User))!
                .FirstOrDefault()!;
        }
        catch
        {
            // ignored
        }

        if (user != null)
        {
            User = user;
        }
    }
}