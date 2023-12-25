using System.Text.Json;
using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.Users.Domain;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;
using TaskStatus = TaskNinjaHub.Application.Entities.TaskStatuses.Domain.TaskStatus;

namespace TaskNinjaHub.WebClient.Components;

/// <summary>
/// Class TaskCreateForm.
/// Implements the <see cref="ComponentBase" />
/// </summary>
/// <seealso cref="ComponentBase" />
public partial class TaskCreateForm
{
    #region INJECTIONS

    /// <summary>
    /// Gets or sets the message.
    /// </summary>
    /// <value>The message.</value>
    [Inject] 
    private IMessageService Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the navigation manager.
    /// </summary>
    /// <value>The navigation manager.</value>
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task service.
    /// </summary>
    /// <value>The task service.</value>
    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;
    
    /// <summary>
    /// Gets or sets the file service.
    /// </summary>
    /// <value>The file service.</value>
    [Inject]
    private FileService FileService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task status service.
    /// </summary>
    /// <value>The task status service.</value>
    [Inject]
    private TaskStatusService TaskStatusService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the role service.
    /// </summary>
    /// <value>The role service.</value>
    [Inject]
    private RoleService RoleService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the priority service.
    /// </summary>
    /// <value>The priority service.</value>
    [Inject]
    private PriorityService PriorityService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the information system service.
    /// </summary>
    /// <value>The information system service.</value>
    [Inject]
    private InformationSystemService InformationSystemService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the author service.
    /// </summary>
    /// <value>The author service.</value>
    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    /// <summary>
    /// Gets or sets the user authentication service.
    /// </summary>
    /// <value>The user authentication service.</value>
    [Inject]
    private IUserAuthenticationService UserAuthenticationService { get; set; } = null!;

    #endregion

    #region PROPERTY

    /// <summary>
    /// Gets the bool definition visibility preview.
    /// </summary>
    /// <value>The preview visibility bool.</value>
    private bool IsPreviewVisible { get; set; }
    
    /// <summary>
    /// Gets the file preview url string.
    /// </summary>
    /// <value>The file preview url.</value>
    private string? FilePreviewUrl { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the file preview title string.
    /// </summary>
    /// <value>The file preview title.</value>
    private string? FilePreviewTitle { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets the current user.
    /// </summary>
    /// <value>The current user.</value>
    private User? CurrentUser => UserAuthenticationService.AuthorizedUser;

    /// <summary>
    /// Gets or sets the created catalog task.
    /// </summary>
    /// <value>The created catalog task.</value>
    private CatalogTask CreatedCatalogTask { get; set; } = new();

    /// <summary>
    /// Gets or sets the authors.
    /// </summary>
    /// <value>The authors.</value>
    private List<Author> AuthorsList { get; set; } = new();

    /// <summary>
    /// Gets or sets the priority list.
    /// </summary>
    /// <value>The priority list.</value>
    private List<Priority> PriorityList { get; set; } = new();

    /// <summary>
    /// Gets or sets the information system list.
    /// </summary>
    /// <value>The information system list.</value>
    private List<InformationSystem> InformationSystemList { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the uploaded files list.
    /// </summary>
    /// <value>The uploaded files list.</value>
    private List<File> UploadedFileList { get; set; } = new();

    /// <summary>
    /// Gets or sets the task status list.
    /// </summary>
    /// <value>The task status list.</value>
    private List<TaskStatus> TaskStatusList { get; set; } = new();

    /// <summary>
    /// Gets or sets the default status.
    /// </summary>
    /// <value>The default status.</value>
    private TaskStatus? DefaultStatus { get; set; } = new();

    #endregion

    /// <summary>
    /// Method invoked when the component is ready to start, having received its
    /// initial parameters from its parent in the render tree.
    /// Override this method if you will perform an asynchronous operation and
    /// want the component to refresh when that operation is completed.
    /// </summary>
    /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> representing any asynchronous operation.</returns>
    protected override async Task OnInitializedAsync()
    {
        AuthorsList = (await AuthorService.GetAllAsync() ?? Array.Empty<Author>()).ToList();
        PriorityList = (await PriorityService.GetAllAsync() ?? Array.Empty<Priority>()).ToList();
        InformationSystemList = (await InformationSystemService.GetAllAsync() ?? Array.Empty<InformationSystem>()).ToList();
        TaskStatusList = (await TaskStatusService.GetAllAsync() ?? Array.Empty<TaskStatus>()).ToList();
        DefaultStatus = TaskStatusList.FirstOrDefault();
    }

    /// <summary>
    /// Creates the task.
    /// </summary>
    private async Task CreateTask()
    {
        CreatedCatalogTask.TaskAuthorId = CurrentUser?.AuthorId;6
        CreatedCatalogTask.TaskStatusId = DefaultStatus?.Id;
        CreatedCatalogTask.UserCreated = CurrentUser?.Username;
        CreatedCatalogTask.DateCreated = DateTime.UtcNow;

        var responseMessage = await CatalogTaskService.CreateAsync(CreatedCatalogTask);

        if (responseMessage.IsSuccessStatusCode)
        {
            var createdTaskStream = await responseMessage.Content.ReadAsStreamAsync();
            var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                createdTaskStream, 
                new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
            foreach (var fileEntity in UploadedFileList)
                await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
            await Message.Success("The task was successfully added.");
        }
        else
        {
            foreach (var fileEntity in UploadedFileList)
                await FileService.DeleteAsync(fileEntity.Id);
            await Message.Error(responseMessage.ReasonPhrase);
        }

        CreatedCatalogTask = new CatalogTask();
        StateHasChanged();
    }

    /// <summary>
    /// Called when [finish].
    /// </summary>
    /// <param name="arg">The argument.</param>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private async Task OnFinish(EditContext arg)
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    /// <summary>
    /// Called when [finish failed].
    /// </summary>
    /// <param name="arg">The argument.</param>
    /// <returns>Task.</returns>
    /// <exception cref="System.NotImplementedException"></exception>
    private async Task OnFinishFailed(EditContext arg)
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    /// <summary>
    /// Called before file upload to server.
    /// </summary>
    /// <param name="file">The input file.</param>
    /// <returns>Task.</returns>
    private bool BeforeUpload(UploadFileItem file)
    {
        var isLt8M = file.Size / 1024 / 1024 < 8;
        if (!isLt8M)
        {
            Message.Error("File must smaller than 8MB!");
        }
        return isLt8M;
    }

    /// <summary>
    /// Called after file change.
    /// </summary>
    /// <param name="fileInfo">The input file info.</param>
    /// <returns>Task.</returns>
    private void HandleChange(UploadInfo fileInfo)
    {
        if (fileInfo.File.State != UploadState.Success) return;
        var uploadedFile = fileInfo.File.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        if (uploadedFile is not null)
            UploadedFileList.Add(uploadedFile);
    }
    
    /// <summary>
    /// Called on file preview.
    /// </summary>
    /// <param name="file">The input file .</param>
    /// <returns>Task.</returns>
    private void OnPreview(UploadFileItem file)
    {
        if (!file.IsPicture())
        {
            NavigationManager.NavigateTo(file.ObjectURL,true);
            return;
        }
        IsPreviewVisible = true;
        FilePreviewTitle = file.FileName;
        FilePreviewUrl = file.ObjectURL;
    }
    
    /// <summary>
    /// Called on file remove.
    /// </summary>
    /// <param name="file">The input file .</param>
    /// <returns>Task.</returns>
    private async Task<bool> OnRemove(UploadFileItem file)
    {
        var uploadedFile = file.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});
        var fileToRemove = UploadedFileList.FirstOrDefault(f => f.Id == uploadedFile!.Id);
        if (fileToRemove is null)
            return false;
        var res = await FileService.DeleteAsync(fileToRemove.Id);
        return res.IsSuccessStatusCode;
    }
}