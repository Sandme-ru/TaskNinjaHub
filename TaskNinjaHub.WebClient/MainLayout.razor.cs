using Microsoft.AspNetCore.Components;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient;

public partial class MainLayout
{
    [Inject]
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        await UserProviderService.GetUser();

        if (UserProviderService.User != null)
        {
            var response = await AuthorService.CreateAsync(UserProviderService.User);

            Console.WriteLine(response.Success ? "Add new author" : response.ErrorMessage);
        }
    }
}