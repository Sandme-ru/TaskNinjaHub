using AntDesign;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Text.Json;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskCreateForm
{
    #region INJECTIONS

    [Inject] 
    private IMessageService Message { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;
    
    [Inject]
    private FileService FileService { get; set; } = null!;

    [Inject]
    private TaskStatusService TaskStatusService { get; set; } = null!;
    
    [Inject]
    private PriorityService PriorityService { get; set; } = null!;

    [Inject]
    private InformationSystemService InformationSystemService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    [Inject]
    private IUserProviderService UserProviderService { get; set; } = null!;

    #endregion

    #region PROPERTY

    private bool IsPreviewVisible { get; set; }
    
    private string? FilePreviewUrl { get; set; } = string.Empty;
    
    private string? FilePreviewTitle { get; set; } = string.Empty;

    private Author? CurrentUser { get; set; } = null!;

    private CatalogTask CreatedCatalogTask { get; set; } = new();

    private List<Author> AuthorsList { get; set; } = new();

    private List<Priority> PriorityList { get; set; } = new();

    private List<InformationSystem> InformationSystemList { get; set; } = new();
    
    private List<File> UploadedFileList { get; set; } = new();

    private List<CatalogTaskStatus> TaskStatusList { get; set; } = new();

    private CatalogTaskStatus? DefaultStatus { get; set; } = new();

    #endregion

    protected override async Task OnInitializedAsync()
    {
        CurrentUser = UserProviderService.User;

        AuthorsList = (await AuthorService.GetAllAsync() ?? Array.Empty<Author>()).ToList();
        PriorityList = (await PriorityService.GetAllAsync() ?? Array.Empty<Priority>()).ToList();
        InformationSystemList = (await InformationSystemService.GetAllAsync() ?? Array.Empty<InformationSystem>()).ToList();
        TaskStatusList = (await TaskStatusService.GetAllAsync() ?? Array.Empty<CatalogTaskStatus>()).ToList();
        DefaultStatus = TaskStatusList.FirstOrDefault();
    }

    private async Task CreateTask()
    {
        CreatedCatalogTask.TaskAuthorId = CurrentUser?.Id;
        CreatedCatalogTask.TaskStatusId = DefaultStatus?.Id;
        CreatedCatalogTask.UserCreated = CurrentUser?.Name;
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

    private async Task OnFinish(EditContext arg)
    {
        Console.WriteLine($"Success:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    private async Task OnFinishFailed(EditContext arg)
    {
        Console.WriteLine($"Failed:{JsonSerializer.Serialize(CreatedCatalogTask)}");
    }

    private bool BeforeUpload(UploadFileItem file)
    {
        var isLt8M = file.Size / 1024 / 1024 < 8;
        if (!isLt8M)
        {
            Message.Error("File must smaller than 8MB!");
        }
        return isLt8M;
    }

    private void HandleChange(UploadInfo fileInfo)
    {
        if (fileInfo.File.State != UploadState.Success) return;

        var uploadedFile = fileInfo.File.GetResponse<File>(new JsonSerializerOptions {PropertyNamingPolicy = JsonNamingPolicy.CamelCase});

        if (uploadedFile is not null)
            UploadedFileList.Add(uploadedFile);
    }
    
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