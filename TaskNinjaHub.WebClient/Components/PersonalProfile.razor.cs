using AntDesign;
using Microsoft.AspNetCore.Components;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.Authors.Dto;
using TaskNinjaHub.Application.Entities.Authors.Enums;
using TaskNinjaHub.WebClient.Services.Bases;
using TaskNinjaHub.WebClient.Services;

namespace TaskNinjaHub.WebClient.Components;

public partial class PersonalProfile
{
    #region INJECTIONS

    [Inject]
    private IUserProviderService UserProviderService { get; set; } = null!;

    [Inject]
    private AuthService AuthService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    [Inject]
    private IMessageService MessageService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    #endregion

    #region PROPERTY

    private bool IsLoading { get; set; }

    private readonly Dictionary<LocalizationType, string> _localizationTypes = new()
    {
        { LocalizationType.Russian, "Русский" },
        { LocalizationType.English, "English"}
    };

    private AuthorDto Author { get; set; } = new();

    #endregion

    #region METHODS

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            IsLoading = true;
            StateHasChanged();

            Author.Name = UserProviderService.User.Name!;
            Author.Id = UserProviderService.User.AuthGuid!;
            Author.LocalizationType = (LocalizationType)UserProviderService.User.LocalizationType!;
            Author.Password = string.Empty;

            IsLoading = false;
            StateHasChanged();
        }
    }

    private async Task EditProfile()
    {
        IsLoading = true;
        StateHasChanged();

        var userEditResult = await AuthService.EditUserAsync(Author);

        var author = (await AuthorService.GetAllByFilterAsync(new Author { AuthGuid = Author.Id })).FirstOrDefault();
        if (author != null)
        {
            author.LocalizationType = Author.LocalizationType;
            author.Name = Author.Name;

            var authorUpdateResult = await AuthorService.UpdateAsync(author);

            switch (userEditResult.Success)
            {
                case true when authorUpdateResult.Success:
                    await MessageService.Success(Localizer["ProfileUpdated"].Value);
                    NavigationManager.NavigateTo("Logout?", true);
                    break;
                case false:
                    await MessageService.Error(userEditResult.Error);
                    break;
            }

            if (!authorUpdateResult.Success)
                await MessageService.Error(authorUpdateResult.ErrorMessage);
        }

        IsLoading = false;
        StateHasChanged();
    }

    #endregion
}