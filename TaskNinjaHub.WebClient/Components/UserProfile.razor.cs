using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;

namespace TaskNinjaHub.WebClient.Components;

public partial class UserProfile
{
    #region INJECTIONS

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [Inject]
    private IMessageService Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user authentication service.
    /// </summary>
    /// <value>The user authentication service.</value>
    [Inject]
    private IUserAuthenticationService UserAuthenticationService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user.
    /// </summary>
    /// <value>The user.</value>
    [Inject]
    private UserService UserService { get; set; } = null!;

    #endregion

    #region PROPERTY

    /// <summary>
    /// The current user.
    /// </summary>
    private User? CurrentUser => UserAuthenticationService.AuthorizedUser;

    /// <summary>
    /// The clone of current user.
    /// </summary>
    private User? _clone;

    /// <summary>
    /// The visible modal
    /// </summary>
    private bool _visibleModal = false;

    /// <summary>
    /// The enable of edit form.
    /// </summary>
    private bool _enableEdit = false;

    /// <summary>
    /// The disable of password repeat input.
    /// </summary>
    private bool _repeatDisabled = true;

    /// <summary>
    /// The password repeat.
    /// </summary>
    private string? repeatPassword = null;

    /// <summary>
    /// The avatar source.
    /// </summary>
    private string? avatar;

    /// <summary>
    /// The new avatar source.
    /// </summary>
    private string? newAvatar;

    /// <summary>
    /// The resized image.
    /// </summary>
    private IBrowserFile? resizedImage;

    #endregion

    /// <summary>
    /// On initialized as an asynchronous operation.
    /// </summary>
    /// <returns>A Task representing the asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        _clone = new User
        {
            Username = CurrentUser.Username,
            Password = CurrentUser.Password,
            AvatarPath = CurrentUser.AvatarPath,
        };

        if (CurrentUser.AvatarPath != null)
        {
            var result = await UserService.GetAvatarAsync(CurrentUser.AvatarPath);
            if (result.IsSuccessStatusCode)
            {
                avatar = result.RequestMessage.RequestUri.ToString();
            }
        }
    }

    /// <summary>
    /// Edits the user.
    /// </summary>
    /// <returns>Task.</returns>
    private async Task EditUser()
    {
        if (!_repeatDisabled && CurrentUser.Password != repeatPassword)
        {
            await Message.Error("Password mismatch");
        }
        else
        {
            var ret = await UserService.UpdateAsync(CurrentUser!);
            _clone.Username = CurrentUser.Username;
            _clone.Password = CurrentUser.Password;

            if (ret.IsSuccessStatusCode)
            {
                await Message.Success("The user was successfully changed.");
                _enableEdit = false;
            }
            else
                await Message.Error(ret.ReasonPhrase);
        }
    }

    /// <summary>
    /// Handles the cancel.
    /// </summary>
    /// <returns>Task.</returns>
    private Task HandleCancel()
    {
        CurrentUser.Username = _clone.Username;
        CurrentUser.Password = _clone.Password;
        repeatPassword = null;
        _enableEdit = false;
        _repeatDisabled = true;
        return Task.CompletedTask;
    }


    /// <summary>
    /// Handles the change of file input.
    /// </summary>
    /// <returns>Task.</returns>
    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        var imageFile = e.File;
        if (imageFile.ContentType != "image/jpeg" && imageFile.ContentType != "image/png")
        {
            await Message.Error("You can only upload JPG/PNG file!");
        }
        else
        {
            resizedImage = await imageFile.RequestImageFileAsync("image/png", 500, 500);

            MemoryStream ms = new MemoryStream();
            await resizedImage.OpenReadStream().CopyToAsync(ms);
            var bytes = ms.ToArray();

            var b64 = Convert.ToBase64String(bytes);

            newAvatar = "data:image/png;base64," + b64;

            _visibleModal = true;

            await HandleCancel();
        }
    }

    /// <summary>
    /// Uloads the avatar.
    /// </summary>
    private async Task UploadAvatar()
    {
        using var content = new MultipartFormDataContent();
        string fileName = Path.GetRandomFileName();

        content.Add(
                content: new StreamContent(resizedImage.OpenReadStream()),
                name: "\"files\"",
                fileName: fileName);

        var response = await UserService.UploadAvatarAsync(content);

        if (response.IsSuccessStatusCode)
        {
            CurrentUser.AvatarPath = fileName;
            _clone.AvatarPath = fileName;
            await UserService.UpdateAsync(CurrentUser!);

            await Message.Success("Upload completed successfully.");
            _visibleModal = false;
            var result = await UserService.GetAvatarAsync(CurrentUser.AvatarPath);
            avatar = result.RequestMessage.RequestUri.ToString();
        }
        else
        {
            await Message.Error("Upload failed.");
            _visibleModal = false;
        }
    }

    /// <summary>
    /// Handles the cancel.
    /// </summary>
    /// <returns>Task.</returns>
    private Task UploadAvatarCancel()
    {
        _visibleModal = false;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Edits the task.
    /// </summary>
    /// <returns>Task.</returns>
    private Task EnableEdit()
    {
        _enableEdit = true;
        return Task.CompletedTask;
    }

    /// <summary>
    /// Handles the change of password input's value.
    /// </summary>
    /// <returns>Task.</returns>
    private Task PasswordChanged()
    {
        _repeatDisabled = false;
        return Task.CompletedTask;
    }
}

