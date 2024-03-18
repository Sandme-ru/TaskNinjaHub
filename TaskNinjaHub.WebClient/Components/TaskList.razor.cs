using AntDesign;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using TaskNinjaHub.Application.Entities.Authors.Domain;
using TaskNinjaHub.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.Application.Entities.Priorities.Domain;
using TaskNinjaHub.Application.Entities.Tasks.Domain;
using TaskNinjaHub.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.Application.Filters;
using TaskNinjaHub.WebClient.Services;
using TaskNinjaHub.WebClient.Services.Bases;
using File = TaskNinjaHub.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.WebClient.Components;

public partial class TaskList
{
    #region INJECTIONS

    [Inject]
    private IMessageService Message { get; set; } = null!;

    [Inject]
    private ModalService ModalService { get; set; } = null!;

    [Inject]
    private FileService FileService { get; set; } = null!;

    [Inject]
    private CatalogTaskService CatalogTaskService { get; set; } = null!;

    [Inject]
    private TaskStatusService TaskStatusService { get; set; } = null!;

    [Inject]
    private PriorityService PriorityService { get; set; } = null!;

    [Inject]
    private InformationSystemService InformationSystemService { get; set; } = null!;

    [Inject]
    private AuthorService AuthorService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IMessageService MessageService { get; set; } = null!;

    [Inject]
    private IUserProviderService UserProviderService { get; set; } = null!;

    #endregion

    #region PROPERTY

    private bool IsPreviewVisible { get; set; }

    private string? FilePreviewUrl { get; set; } = string.Empty;

    private string? FilePreviewTitle { get; set; } = string.Empty;

    private string? CurrentUser
    {
        get => UserProviderService.User.Name;
        set => UserProviderService.User.Name = value;
    }

    private CatalogTask? EditedTask { get; set; }

    private CatalogTask? CloneTask { get; set; }

    private CatalogTask? CatalogTaskForChangelog { get; set; }

    private List<CatalogTask>? CatalogTasks { get; set; }

    private List<UploadFileItem>? DefaultFileList { get; set; } = new();

    private readonly ListGridType _grid = new()
    {
        Gutter = 16,
        Xs = 1,
        Sm = 2,
        Md = 4,
        Lg = 4,
        Xl = 6,
        Xxl = 3,
    };


    private bool _visibleModal = false;
    
    private IEnumerable<Author>? AuthorsList { get; set; }

    private IEnumerable<Priority>? PriorityList { get; set; }

    private IEnumerable<InformationSystem>? InformationSystemList { get; set; }

    private IEnumerable<CatalogTaskStatus>? TaskStatusList { get; set; }

    private static CatalogTask? DeletedTask { get; set; }

    private Func<ModalClosingEventArgs, Task> _onOk;

    private InformationSystem? SelectedInformationSystem { get; set; }

    private Author? SelectedAuthor { get; set; }

    private Author? SelectedExecutor { get; set; }

    private int? SelectedTaskId { get; set; }

    private int PageSize { get; set; } = 10;

    private int CurrentPage { get; set; } = 1;

    private int CatalogTasksCount { get; set; }

    private bool IsLoadingTaskList { get; set; }

    #endregion
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(firstRender)
        {
            IsLoadingTaskList = true;
            StateHasChanged();

            CatalogTasksCount = await CatalogTaskService.GetAllCountAsync();

            AuthorsList = await AuthorService.GetAllAsync();
            PriorityList = await PriorityService.GetAllAsync();
            InformationSystemList = await InformationSystemService.GetAllAsync();
            TaskStatusList = await TaskStatusService.GetAllAsync();
            CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel
                { PageNumber = CurrentPage, PageSize = PageSize })).ToList();

            IsLoadingTaskList = false;
            StateHasChanged();
        }
    }

    private async Task DeleteTaskHandler()
    {
        if (DeletedTask != null)
        {
            if (CatalogTasks != null)
            {
                foreach (var task in CatalogTasks.Where(t => t.OriginalTaskId == DeletedTask.Id))
                    await CatalogTaskService.DeleteAsync(task.Id);

                var response = await CatalogTaskService.DeleteAsync(DeletedTask.Id);
                if (response.IsSuccessStatusCode)
                {
                    CatalogTasks.Remove(DeletedTask);
                    await MessageService.Success("Deletion completed successfully.");
                }
                else
                    await MessageService.Error("Deletion failed.");
            }
        }

        StateHasChanged();
    }

    private Func<ModalClosingEventArgs, Task> _onCancel = (e) =>
    {
        Console.WriteLine("Cancel");
        return Task.CompletedTask;
    };

    public TaskList()
    {
        _onOk = async (e) =>
        {
            await DeleteTaskHandler();
            Console.WriteLine("OK");
        };
    }

    private async Task EditTaskEnabled(CatalogTask? catalogTask)
    {
        EditedTask = catalogTask;
        var taskFiles = await FileService.GetAllByTaskIdAsync(catalogTask!.Id);
        if (taskFiles?.ToList() is not null and { Count: > 0 } files)
        {
            catalogTask.Files = files;
            DefaultFileList = catalogTask.Files
                .Select(f => new UploadFileItem
                {
                    Id = $"{f.Id}",
                    FileName = f.Name,
                    Url = $"https://localhost:7179/{f.Path}", //TODO remove connection to localhost
                    ObjectURL = $"https://localhost:7179/{f.Path}",
                    Response = JsonSerializer.Serialize(
                        f,
                        new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
                    State = UploadState.Success
                }).ToList()!;
        }
        else
        {
            EditedTask!.Files = [];
            DefaultFileList = [];
        }

        CloneTask = new CatalogTask
        {
            Name = catalogTask.Name,
            Description = catalogTask.Description,
            TaskAuthorId = catalogTask.TaskAuthorId,
            TaskExecutorId = catalogTask.TaskExecutorId,
            InformationSystemId = catalogTask.InformationSystemId,
            PriorityId = catalogTask.PriorityId,
            TaskStatusId = catalogTask.TaskStatusId,
            Files = catalogTask.Files
        };

        _visibleModal = true;

        StateHasChanged();
    }

    private Task HandleCancel()
    {
        if (EditedTask != null)
        {
            EditedTask.Name = CloneTask?.Name ?? string.Empty;
            EditedTask.Description = CloneTask?.Description;
            EditedTask.TaskAuthorId = CloneTask?.TaskAuthorId;
            EditedTask.TaskExecutorId = CloneTask?.TaskExecutorId;
            EditedTask.InformationSystemId = CloneTask?.InformationSystemId;
            EditedTask.PriorityId = CloneTask?.PriorityId;
            EditedTask.TaskStatusId = CloneTask?.TaskStatusId;
        }

        _visibleModal = false;
        StateHasChanged();
        return Task.CompletedTask;
    }

    private async Task EditTask()
    {
        if (EditedTask != null)
        {
            if (EditedTask.Name == CloneTask?.Name
                && EditedTask.Description == CloneTask?.Description
                && EditedTask.TaskAuthorId == CloneTask?.TaskAuthorId
                && EditedTask.TaskExecutorId == CloneTask?.TaskExecutorId
                && EditedTask.InformationSystemId == CloneTask?.InformationSystemId
                && EditedTask.PriorityId == CloneTask?.PriorityId
                && EditedTask.TaskStatusId == CloneTask?.TaskStatusId
                && Equals(EditedTask.Files?.ToArray(), DefaultFileList?.ToArray()))
            {
                await MessageService.Error("Change one or more attributes");
            }
            else
            {
                Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                UpdatingDependentAttributes();

                EditedTask!.UserUpdated = CurrentUser;
                EditedTask.DateUpdated = DateTime.Now;

                var changeRet = await CatalogTaskService.UpdateAsync(EditedTask!);
                if (changeRet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");

                    if (EditedTask?.Files is { Count: > 0 })
                    {
                        var createdTaskStream = await changeRet.Content.ReadAsStreamAsync();
                        var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                            createdTaskStream,
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                        foreach (var fileEntity in EditedTask.Files!)
                            await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
                    }
                    else
                        foreach (var fileEntity in EditedTask?.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);
                }

                CatalogTaskForChangelog = new CatalogTask
                {
                    Name = CloneTask?.Name,
                    Description = CloneTask?.Description,
                    TaskAuthorId = CloneTask?.TaskAuthorId,
                    TaskExecutorId = CloneTask?.TaskExecutorId,
                    InformationSystemId = CloneTask?.InformationSystemId,
                    PriorityId = CloneTask?.PriorityId,
                    TaskStatusId = CloneTask?.TaskStatusId,
                    OriginalTaskId = EditedTask.Id,
                    DateCreated = EditedTask.DateUpdated,
                    UserCreated = EditedTask.UserUpdated,
                    Files = EditedTask.Files
                };

                var createRet = await CatalogTaskService.CreateAsync(CatalogTaskForChangelog);
                if (createRet.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{EditedTask?.Id} {EditedTask?.Name} is updated.");
                    if (CatalogTaskForChangelog.Files is { Count: > 0 })
                    {
                        var createdTaskStream = await changeRet.Content.ReadAsStreamAsync();
                        var createdTask = await JsonSerializer.DeserializeAsync<CatalogTask>(
                            createdTaskStream,
                            new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                            });
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.ChangeOwnershipAsync(fileEntity.Id, createdTask!.Id);
                    }
                    else
                        foreach (var fileEntity in CatalogTaskForChangelog.Files!)
                            await FileService.DeleteAsync(fileEntity.Id);

                    CatalogTasks = (await CatalogTaskService.GetAllAsync()).ToList();
                }
                _visibleModal = false;
                StateHasChanged();
            }
        }
    }

    private void UpdatingDependentAttributes()
    {
        if (EditedTask == null)
            return;

        if (AuthorsList != null)
        {
            EditedTask.TaskAuthor = AuthorsList.FirstOrDefault(a => a.Id == EditedTask.TaskAuthorId);
            EditedTask.TaskExecutor = AuthorsList.FirstOrDefault(a => a.Id == EditedTask.TaskExecutorId);
        }

        if (InformationSystemList != null)
            EditedTask.InformationSystem = InformationSystemList.FirstOrDefault(a => a.Id == EditedTask.InformationSystemId);
        if (PriorityList != null) 
            EditedTask.Priority = PriorityList.FirstOrDefault(a => a.Id == EditedTask.PriorityId);
        if (TaskStatusList != null)
            EditedTask.TaskStatus = TaskStatusList.FirstOrDefault(a => a.Id == EditedTask.TaskStatusId);
    }

    private void DeleteTask(CatalogTask task)
    {
        DeletedTask = task;
        ModalService.Confirm(new ConfirmOptions
        {
            Title = $"Are you sure delete this task {task.Id} {task.Name}?",
            Icon = _icon,
            Content = "Some descriptions",
            OnOk = _onOk,
            OnCancel = _onCancel,
            OkType = "danger"
        });
    }

    private async Task FilterSelectionChanged()
    {
        IsLoadingTaskList = true;
        StateHasChanged();

        var filter = new CatalogTask
        {
            Id = SelectedTaskId ?? 0,
            InformationSystemId = SelectedInformationSystem?.Id,
            TaskAuthorId = SelectedAuthor?.Id,
            TaskExecutorId = SelectedExecutor?.Id
        };

        var result = (await CatalogTaskService.GetAllByFilterByPageAsync(filter, CurrentPage, PageSize))!
            .Where(t => t.OriginalTaskId == null).ToList();

        CatalogTasks = new List<CatalogTask>(result);

        CatalogTasksCount = CatalogTasks.Count;

        IsLoadingTaskList = false;
        StateHasChanged();
    }

    private bool BeforeUpload(UploadFileItem file)
    {
        var isLt8M = file.Size / 1024 / 1024 < 8;

        if (!isLt8M)
            Message.Error("File must smaller than 8MB!");

        return isLt8M;
    }

    private void HandleChange(UploadInfo fileInfo)
    {
        if (fileInfo.File.State != UploadState.Success)
            return;

        var uploadedFile = fileInfo.File.GetResponse<File>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        if (uploadedFile is not null)
            EditedTask?.Files?.Add(uploadedFile);
    }

    private void OnPreview(UploadFileItem file)
    {
        if (!file.IsPicture())
        {
            NavigationManager.NavigateTo(file.ObjectURL, true);
            return;
        }

        IsPreviewVisible = true;
        FilePreviewTitle = file.FileName;
        FilePreviewUrl = file.ObjectURL;
    }

    private async Task<bool> OnRemove(UploadFileItem file)
    {
        var uploadedFile = file.GetResponse<File>(new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        var fileToRemove = EditedTask?.Files?.FirstOrDefault(f => f.Id == uploadedFile!.Id);

        if (fileToRemove is null)
            return false;

        DefaultFileList?.Remove(file);
        EditedTask?.Files?.Remove(fileToRemove);

        var res = await FileService.DeleteAsync(fileToRemove.Id);
        return res.IsSuccessStatusCode;
    }


    private async Task HandlePageChange(PaginationEventArgs arg)
    {
        IsLoadingTaskList = true;
        StateHasChanged();

        CurrentPage = arg.Page;
        PageSize = arg.PageSize;

        CatalogTasks = (await CatalogTaskService.GetAllByPageAsync(new FilterModel { PageSize = PageSize, PageNumber = CurrentPage }) ?? Array.Empty<CatalogTask>()).ToList();

        IsLoadingTaskList = false;
        StateHasChanged();
    }

    private async Task Open(CatalogTask context)
    {
        NavigationManager.NavigateTo($"task-read/{context.Id}");
        StateHasChanged();
    }
}